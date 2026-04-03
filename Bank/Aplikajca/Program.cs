using Bank;
using System;

Console.WriteLine("Sprawdzanie konta zwykłego\n");

Konto konto1 = new Konto("Bogdan Brzozka", 1200);
Console.WriteLine($"Właściciel: {konto1.Nazwa}, Początkowe środki: {konto1.Bilans} zł");

konto1.Wplata(300);
Console.WriteLine($"Wpłaciłem 300 zł, teraz mam: {konto1.Bilans} zł");

konto1.Wyplata(200);
Console.WriteLine($"Pobrałem 200 zł, zostało mi: {konto1.Bilans} zł\n");

Console.WriteLine("Test blokady konta\n");

Konto konto2 = new Konto("Wiesia Ptok", 800);
Console.WriteLine($"Właściciel: {konto2.Nazwa}, Saldo: {konto2.Bilans} zł");

konto2.BlokujKonto();
Console.WriteLine($"Konto zablokowane? {konto2.Zablokowane}");

try
{
    Console.WriteLine("Próbuję wybrać kasę ze zablokowanego konta...");
    konto2.Wyplata(100);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Nie poszło: {ex.Message}");
}

konto2.OdblokujKonto();
Console.WriteLine($"Konto już odblokowane: {!konto2.Zablokowane}\n");

Console.WriteLine("Seria operacji na koncie\n");

Konto konto3 = new Konto("Bogdan Brzozka", 2500);
Console.WriteLine($"Właściciel: {konto3.Nazwa}, Mam: {konto3.Bilans} zł");

konto3.Wyplata(500);
Console.WriteLine($"Wybrałem 500 zł: {konto3.Bilans} zł");

konto3.Wplata(750);
Console.WriteLine($"Wpłaciłem 750 zł: {konto3.Bilans} zł");

konto3.Wyplata(400);
Console.WriteLine($"Wybrałem 400 zł: {konto3.Bilans} zł");

Console.WriteLine("\nNaciśnij coś żeby wyjść.");
Console.ReadLine();