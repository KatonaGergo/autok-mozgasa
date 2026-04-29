using System;
using System.Collections.Generic;
using System.IO;

class Autok
{
    struct Jeladas
    {
        public string Rendszam;
        public int    Ora;
        public int    Perc;
        public int    Sebesseg;
    }

    static void Main()
    {
        // 1. FELADAT
        var adatok = new List<Jeladas>();
        foreach (string sor in File.ReadAllLines("jeladas.txt"))
        {
            string[] reszek = sor.Trim().Split('\t');
            if (reszek.Length != 4) continue;
            adatok.Add(new Jeladas
            {
                Rendszam = reszek[0],
                Ora      = int.Parse(reszek[1]),
                Perc     = int.Parse(reszek[2]),
                Sebesseg = int.Parse(reszek[3])
            });
        }

        // 2. FELADAT
        Jeladas utolso = adatok[adatok.Count - 1];
        Console.WriteLine("2. feladat:");
        Console.WriteLine($"Az utolso jeladas idopontja {utolso.Ora}:{utolso.Perc:D2}, a jarmu rendszama {utolso.Rendszam}");
        Console.WriteLine();

        // 3. FELADAT
        string elsoR = adatok[0].Rendszam;
        var idopontok = new List<string>();
        foreach (var j in adatok)
        {
            if (j.Rendszam == elsoR)
            {
                idopontok.Add($"{j.Ora}:{j.Perc}");
            }
        }
        string idok = string.Join(" ", idopontok);
        Console.WriteLine("3. feladat:");
        Console.WriteLine($"Az elso jarmu: {elsoR}");
        Console.WriteLine($"Jeladásainak idopontjai: {idok}");
        Console.WriteLine();

        // 4. FELADAT
        Console.WriteLine("4. feladat:");
        Console.Write("Kerem, adja meg az orat: ");
        int kOra  = int.Parse(Console.ReadLine()!);
        Console.Write("Kerem, adja meg a percet: ");
        int kPerc = int.Parse(Console.ReadLine()!);
        int db = 0;
        foreach (var j in adatok)
        {
            if (j.Ora == kOra && j.Perc == kPerc)
            {
                db++;
            }
        }
        Console.WriteLine($"A jeladasok szama: {db}");
        Console.WriteLine();

        // 5. FELADAT
        int maxSeb = adatok[0].Sebesseg;
        foreach (var j in adatok)
        {
            if (j.Sebesseg > maxSeb)
            {
                maxSeb = j.Sebesseg;
            }
        }
        var maxJarmuvek = new List<string>();
        foreach (var j in adatok)
        {
            if (j.Sebesseg == maxSeb)
            {
                maxJarmuvek.Add(j.Rendszam);
            }
        }
        Console.WriteLine("5. feladat:");
        Console.WriteLine($"A legnagyobb sebesseg km/h: {maxSeb}");
        Console.WriteLine($"A jarmuvek: {string.Join(" ", maxJarmuvek)}");
        Console.WriteLine();

        // 6. FELADAT
        Console.WriteLine("6. feladat:");
        Console.Write("Kerem, adja meg a rendszamot: ");
        string kR = Console.ReadLine()!.Trim();
        var cSigs = new List<Jeladas>();
        foreach (var j in adatok)
        {
            if (j.Rendszam == kR)
            {
                cSigs.Add(j);
            }
        }
        if (cSigs.Count == 0)
        {
            Console.WriteLine("Nincs ilyen rendszamu jarmu az adatokban.");
        }
        else
        {
            double tav = 0.0;
            for (int i = 0; i < cSigs.Count; i++)
            {
                var j = cSigs[i];
                Console.WriteLine($"{j.Ora}:{j.Perc} {tav:F1} km");
                if (i + 1 < cSigs.Count)
                {
                    var k = cSigs[i + 1];
                    double elteltOra = (k.Ora * 60 + k.Perc - j.Ora * 60 - j.Perc) / 60.0;
                    tav += j.Sebesseg * elteltOra;
                }
            }
        }
        Console.WriteLine();

        // 7. FELADAT
        var sorrendben    = new List<string>();
        var elsoJeladas   = new Dictionary<string, (int o, int p)>();
        var utolsoJeladas = new Dictionary<string, (int o, int p)>();
        foreach (var j in adatok)
        {
            if (!elsoJeladas.ContainsKey(j.Rendszam))
            {
                sorrendben.Add(j.Rendszam);
                elsoJeladas[j.Rendszam] = (j.Ora, j.Perc);
            }
            utolsoJeladas[j.Rendszam] = (j.Ora, j.Perc);
        }
        using (var fw = new StreamWriter("ido.txt"))
            foreach (string r in sorrendben)
            {
                var (fo, fp) = elsoJeladas[r];
                var (lo, lp) = utolsoJeladas[r];
                fw.WriteLine($"{r} {fo} {fp} {lo} {lp}");
            }
        Console.WriteLine("7. feladat:");
        Console.WriteLine("Az ido.txt allomany sikeresen elkeszult.");
    }
}
