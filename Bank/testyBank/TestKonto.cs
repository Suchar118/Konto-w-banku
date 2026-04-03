using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bank;

namespace Bank.Tests;

[TestClass]
public class KontoTests
{
    [TestMethod]
    public void Konstruktor_Tworzy_Konto_Z_Prawidlowymi_Danymi()
    {
        var konto = new Konto("Bogdan Brzozka", 500);

        Assert.AreEqual("Bogdan Brzozka", konto.Nazwa);
        Assert.AreEqual(500, konto.Bilans);
        Assert.IsFalse(konto.Zablokowane);
    }

    [TestMethod]
    public void Konstruktor_Rzuca_Blad_Gdy_Nazwa_Pusta()
    {
        Assert.Throws<ArgumentException>(() => new Konto(""));
        Assert.Throws<ArgumentException>(() => new Konto(null));
    }

    [TestMethod]
    public void Konstruktor_Rzuca_Blad_Na_Ujemny_Bilans()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Konto("Wiesia Ptok", -50));
    }

    [TestMethod]
    public void BlokujKonto_Prawidlowo_Blokuje()
    {
        var konto = new Konto("Bogdan Brzozka", 300);
        konto.BlokujKonto();
        Assert.IsTrue(konto.Zablokowane);
    }

    [TestMethod]
    public void OdblokujKonto_Prawidlowo_Odblokowuje()
    {
        var konto = new Konto("Wiesia Ptok", 400);
        konto.BlokujKonto();
        konto.OdblokujKonto();
        Assert.IsFalse(konto.Zablokowane);
    }

    [TestMethod]
    public void Wplata_Zwieksza_Saldo()
    {
        var konto = new Konto("Bogdan Brzozka", 600);
        konto.Wplata(200);
        Assert.AreEqual(800, konto.Bilans);
    }

    [TestMethod]
    public void Wplata_Blad_Na_Nieprawidlowa_Kwote()
    {
        var konto = new Konto("Wiesia Ptok", 500);
        Assert.Throws<ArgumentOutOfRangeException>(() => konto.Wplata(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => konto.Wplata(-25));
    }

    [TestMethod]
    public void Wplata_Blad_Gdy_Konto_Zablokowane()
    {
        var konto = new Konto("Bogdan Brzozka", 350);
        konto.BlokujKonto();
        Assert.Throws<InvalidOperationException>(() => konto.Wplata(100));
    }

    [TestMethod]
    public void Wyplata_Zmniejsza_Saldo()
    {
        var konto = new Konto("Wiesia Ptok", 1000);
        konto.Wyplata(300);
        Assert.AreEqual(700, konto.Bilans);
    }

    [TestMethod]
    public void Wyplata_Blad_Na_Nieprawidlowa_Kwote()
    {
        var konto = new Konto("Bogdan Brzozka", 800);
        Assert.Throws<ArgumentOutOfRangeException>(() => konto.Wyplata(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => konto.Wyplata(-75));
    }

    [TestMethod]
    public void Wyplata_Blad_Gdy_Konto_Zablokowane()
    {
        var konto = new Konto("Wiesia Ptok", 600);
        konto.BlokujKonto();
        Assert.Throws<InvalidOperationException>(() => konto.Wyplata(150));
    }

    [TestMethod]
    public void Wyplata_Blad_Gdy_Brak_Srodkow()
    {
        var konto = new Konto("Bogdan Brzozka", 400);
        Assert.Throws<InvalidOperationException>(() => konto.Wyplata(500));
    }
}
