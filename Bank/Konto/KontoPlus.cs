using System;

namespace Bank;

public class KontoPlus : Konto
{
    private decimal limitDebetu;
    private decimal saldoNegacyjne = 0;
    private bool limitJuzUzyty = false;

    public KontoPlus(string wlasciciel, decimal saldoStartowe = 0, decimal limitDebetu = 500)
        : base(wlasciciel, saldoStartowe)
    {
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

    public override decimal Bilans
    {
        get
        {
            if (Zablokowane)
            {
                return 0;
            }

            if (limitJuzUzyty)
            {
                return base.Bilans;
            }

            return base.Bilans + limitDebetu;
        }
    }

    public override void Wplata(decimal kwota)
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
                    base.Wplata(pozostalaSuma);
                }

                if (base.Bilans > 0)
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
            base.Wplata(kwota);
        }

        if (saldoNegacyjne > 0 && kontoByloZablokowane)
        {
            BlokujKonto();
        }
    }

    public override void Wyplata(decimal kwota)
    {
        if (Zablokowane)
        {
            throw new InvalidOperationException("Wypłata niedozwolona - konto jest zablokowane.");
        }

        if (kwota <= 0)
        {
            throw new ArgumentOutOfRangeException("Kwota do wypłacenia musi być dodatnia.");
        }

        if (kwota <= base.Bilans)
        {
            base.Wyplata(kwota);
        }
        else
        {
            if (limitJuzUzyty)
            {
                throw new InvalidOperationException("Twój limit debetowy już został w pełni wykorzystany.");
            }

            decimal niedobor = kwota - base.Bilans;

            if (niedobor > limitDebetu)
            {
                throw new InvalidOperationException("Żądana kwota przewyższa dostępny limit debetowy.");
            }

            if (base.Bilans > 0)
            {
                base.Wyplata(base.Bilans);
            }

            saldoNegacyjne = niedobor;
            limitJuzUzyty = true;

            BlokujKonto();
        }
    }
}