using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bank;

namespace Bank.Tests;

[TestClass]
public class KontoLimitTests
{
    [TestMethod]
    public void LimitDebetu_Blad_Gdy_Ujemny()
    {
        var konto = new KontoLimit("Bogdan Brzozka", 500, 400);
        Assert.Throws<ArgumentOutOfRangeException>(() => konto.LimitDebetu = -200);
    }

    [TestMethod]
    public void Bilans_Zwraca_Zero_Gdy_Zablokowane()
    {
        var konto = new KontoLimit("Wiesia Ptok", 800, 300);
        konto.BlokujKonto();
        Assert.AreEqual(0, konto.Bilans);
    }

    [TestMethod]
    public void Wplata_Blad_Przy_Nieprawidlowej_Kwocie()
    {
        var konto = new KontoLimit("Bogdan Brzozka", 600, 250);

        Assert.Throws<ArgumentOutOfRangeException>(() => konto.Wplata(-150));
        Assert.Throws<ArgumentOutOfRangeException>(() => konto.Wplata(0));
    }

    [TestMethod]
    public void Wplata_Zwieksza_Bilans_Gdy_Brak_Debetu()
    {
        var konto = new KontoLimit("Wiesia Ptok", 0, 300);
        konto.Wplata(200);
        Assert.AreEqual(500, konto.Bilans);
    }

    [TestMethod]
    public void Wplata_Mniejsza_Niz_Niedobor_Pozostawia_Konto_Zablokowane()
    {
        var konto = new KontoLimit("Bogdan Brzozka", 0, 400);
        konto.Wyplata(200);
        konto.Wplata(80);

        Assert.IsTrue(konto.Zablokowane);
        Assert.AreEqual(0, konto.Bilans);
    }

    [TestMethod]
    public void Wplata_Rowna_Niedoborowi_Odblokowuje_Limit_Uzywany()
    {
        var konto = new KontoLimit("Wiesia Ptok", 0, 350);
        konto.Wyplata(150);
        konto.Wplata(150);

        Assert.IsFalse(konto.Zablokowane);
        Assert.AreEqual(0, konto.Bilans);
    }

    [TestMethod]
    public void Wplata_Wieksza_Niz_Niedobor_Odblokowuje_Przywraca_Limit()
    {
        var konto = new KontoLimit("Bogdan Brzozka", 0, 400);
        konto.Wyplata(300);
        konto.Wplata(350);

        Assert.IsFalse(konto.Zablokowane);
        Assert.AreEqual(350, konto.Bilans);
    }

    [TestMethod]
    public void Wyplata_Blad_Gdy_Konto_Zablokowane()
    {
        var konto = new KontoLimit("Wiesia Ptok", 700, 400);
        konto.BlokujKonto();
        Assert.Throws<InvalidOperationException>(() => konto.Wyplata(150));
    }

    [TestMethod]
    public void Wyplata_Blad_Przy_Nieprawidlowej_Kwocie()
    {
        var konto = new KontoLimit("Bogdan Brzozka", 600, 300);
        Assert.Throws<ArgumentOutOfRangeException>(() => konto.Wyplata(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => konto.Wyplata(-200));
    }

    [TestMethod]
    public void Wyplata_Blokuje_Konto_Gdy_Uzyty_Limit()
    {
        var konto = new KontoLimit("Wiesia Ptok", 300, 350);
        konto.Wyplata(600);

        Assert.IsTrue(konto.Zablokowane);
    }

    [TestMethod]
    public void Wyplata_Blad_Gdy_Przekroczony_Limit()
    {
        var konto = new KontoLimit("Bogdan Brzozka", 400, 300);
        Assert.Throws<InvalidOperationException>(() => konto.Wyplata(900));
    }

    [TestMethod]
    public void Wyplata_Blad_Gdy_Limit_Juz_Wykorzystany()
    {
        var konto = new KontoLimit("Wiesia Ptok", 300, 350);
        konto.Wyplata(600);
        konto.Wplata(200);

        Assert.Throws<InvalidOperationException>(() => konto.Wyplata(50));
    }
}