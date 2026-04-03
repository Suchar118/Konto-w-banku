using System;

namespace Bank;

public class Konto
{
    private string klient;
    private decimal bilans;
    private bool zablokowane = false;

    private Konto()
    {
    }

    public Konto(string klient, decimal bilansNaStart = 0)
    {
        if (string.IsNullOrWhiteSpace(klient))
        {
            throw new ArgumentException("Nazwa klienta jest wymagana.");
        }

        if (bilansNaStart < 0)
        {
            throw new ArgumentOutOfRangeException("Początkowy bilans nie może być ujemny.");
        }

        this.klient = klient;
        this.bilans = bilansNaStart;
    }

    public string Nazwa => klient;
    public virtual decimal Bilans => bilans;
    public bool Zablokowane => zablokowane;

    public void BlokujKonto()
    {
        zablokowane = true;
    }

    public void OdblokujKonto()
    {
        zablokowane = false;
    }

    public virtual void Wplata(decimal kwota)
    {
        if (zablokowane)
        {
            throw new InvalidOperationException("Wpłata niedozwolona - konto jest zablokowane.");
        }

        if (kwota <= 0)
        {
            throw new ArgumentOutOfRangeException("Kwota wpłaty musi być dodatnia.");
        }

        bilans += kwota;
    }

    public virtual void Wyplata(decimal kwota)
    {
        if (zablokowane)
        {
            throw new InvalidOperationException("Wypłata niedozwolona - konto jest zablokowane.");
        }

        if (kwota <= 0)
        {
            throw new ArgumentOutOfRangeException("Kwota wypłaty musi być dodatnia.");
        }

        if (kwota > bilans)
        {
            throw new InvalidOperationException("Niewystarczające saldo na koncie.");
        }

        bilans -= kwota;
    }
}