using System;

namespace Bank;

public class KontoLimit
{
    private Konto konto;

    private decimal limitDebetu;
    private decimal saldoNegacyjne = 0;
    private bool limitJuzUzyty;

    public KontoLimit(string wlasciciel, decimal saldoStartowe = 0, decimal limitDebetu = 500)
    {
        this.konto = new Konto(wlasciciel, saldoStartowe);
        LimitDebetu = limitDebetu;
    }

    public decimal LimitDebetu
    {
        get => limitDebetu;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("Kwota limitu nie może być ujemna.");
            }

            limitDebetu = value;
        }
    }

    public string Nazwa => konto.Nazwa;

    public bool Zablokowane => konto.Zablokowane;

    public decimal Bilans
    {
        get
        {
            if (Zablokowane)
                return 0;

            if (limitJuzUzyty)
                return konto.Bilans;

            return konto.Bilans + limitDebetu;
        }
    }

    public void BlokujKonto()
    {
        konto.BlokujKonto();
    }

    public void OdblokujKonto()
    {
        konto.OdblokujKonto();
    }

    public void Wplata(decimal kwota)
    {
        if (kwota <= 0)
        {
            throw new ArgumentOutOfRangeException("Kwota do wpłacenia musi być dodatnia.");
        }

        bool kontoByloZablokowane = Zablokowane;

        if (kontoByloZablokowane)
        {
            OdblokujKonto();
        }

        if (saldoNegacyjne > 0)
        {
            if (kwota >= saldoNegacyjne)
            {
                decimal pozostalaSuma = kwota - saldoNegacyjne;
                saldoNegacyjne = 0;

                if (pozostalaSuma > 0)
                {
                    konto.Wplata(pozostalaSuma);
                }

                if (konto.Bilans > 0)
                {
                    limitJuzUzyty = false;
                }
            }
            else
            {
                saldoNegacyjne -= kwota;
                BlokujKonto();
            }
        }
        else
        {
            konto.Wplata(kwota);
        }

        if (saldoNegacyjne > 0 && kontoByloZablokowane)
        {
            BlokujKonto();
        }
    }

    public void Wyplata(decimal kwota)
    {
        if (Zablokowane)
        {
            throw new InvalidOperationException("Wypłata niedozwolona - konto jest zablokowane.");
        }

        if (kwota <= 0)
        {
            throw new ArgumentOutOfRangeException("Kwota do wypłacenia musi być dodatnia.");
        }

        if (kwota <= konto.Bilans)
        {
            konto.Wyplata(kwota);
        }
        else
        {
            if (limitJuzUzyty)
            {
                throw new InvalidOperationException("Twój limit debetowy już został w pełni wykorzystany.");
            }

            decimal niedobor = kwota - konto.Bilans;

            if (niedobor > limitDebetu)
            {
                throw new InvalidOperationException("Żądana kwota przewyższa dostępny limit debetowy.");
            }

            if (konto.Bilans > 0)
            {
                konto.Wyplata(konto.Bilans);
            }

            saldoNegacyjne = niedobor;
            limitJuzUzyty = true;
            BlokujKonto();
        }
    }
}