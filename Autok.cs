// ============================================================
// 3. Autók mozgása – megoldás (C#)
// Érettségi vizsga 2024. október 22.
// ============================================================
// Bemeneti fájl: jeladas.txt  (tabulátorral elválasztva)
// Oszlopok: rendszám  óra  perc  sebesség(km/h)
// ============================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Autok
{
    struct Jeladas
    {
        public string Rendszam;
        public int    Ora;
        public int    Perc;
        public int    Sebesseg; // km/h
    }

    static void Main()
    {
        // ------------------------------------------------------------------
        // 1. FELADAT – Adatok beolvasása és tárolása
        // ------------------------------------------------------------------
        var adatok = new List<Jeladas>();

        foreach (string sor in File.ReadAllLines("jeladas.txt"))
        {
            string[] reszek = sor.Trim().Split('\t');
            if (reszek.Length != 4) continue;

            adatok.Add(new Jeladas
            {
                Rendszam  = reszek[0],
                Ora       = int.Parse(reszek[1]),
                Perc      = int.Parse(reszek[2]),
                Sebesseg  = int.Parse(reszek[3])
            });
        }

        // ------------------------------------------------------------------
        // 2. FELADAT – Utolsó jeladás időpontja és járműve
        // ------------------------------------------------------------------
        Jeladas utolso = adatok[adatok.Count - 1];
        Console.WriteLine("2. feladat:");
        Console.WriteLine(
            $"Az utolsó jeladás időpontja {utolso.Ora}:{utolso.Perc:D2}, " +
            $"a jármű rendszáma {utolso.Rendszam}");
        Console.WriteLine();

        // ------------------------------------------------------------------
        // 3. FELADAT – Első jármű rendszáma és jeladásainak időpontjai
        // ------------------------------------------------------------------
        string elsoRendszam = adatok[0].Rendszam;
        var elsoJeladasok   = adatok.Where(j => j.Rendszam == elsoRendszam).ToList();
        string idopontok    = string.Join(" ",
            elsoJeladasok.Select(j => $"{j.Ora}:{j.Perc}"));

        Console.WriteLine("3. feladat:");
        Console.WriteLine($"Az első jármű: {elsoRendszam}");
        Console.WriteLine($"Jeladásainak időpontjai: {idopontok}");
        Console.WriteLine();

        // ------------------------------------------------------------------
        // 4. FELADAT – Jeladások száma adott időpontban
        // ------------------------------------------------------------------
        Console.WriteLine("4. feladat:");
        Console.Write("Kérem, adja meg az órát: ");
        int kertOra  = int.Parse(Console.ReadLine()!);
        Console.Write("Kérem, adja meg a percet: ");
        int kertPerc = int.Parse(Console.ReadLine()!);

        int db = adatok.Count(j => j.Ora == kertOra && j.Perc == kertPerc);
        Console.WriteLine($"A jeladások száma: {db}");
        Console.WriteLine();

        // ------------------------------------------------------------------
        // 5. FELADAT – Legnagyobb sebesség és a hozzá tartozó járművek
        // ------------------------------------------------------------------
        int maxSeb       = adatok.Max(j => j.Sebesseg);
        var maxJarmuvek  = adatok
            .Where(j => j.Sebesseg == maxSeb)
            .Select(j => j.Rendszam);

        Console.WriteLine("5. feladat:");
        Console.WriteLine($"A legnagyobb sebesség km/h: {maxSeb}");
        Console.WriteLine($"A járművek: {string.Join(" ", maxJarmuvek)}");
        Console.WriteLine();

        // ------------------------------------------------------------------
        // 6. FELADAT – Adott jármű távolsága az útszakasz elejétől
        // ------------------------------------------------------------------
        // Az autó a jeladástól a következő jeladásig az akkori sebességgel halad.
        // Megtett út = sebesség × időkülönbség (órában kifejezve).
        Console.WriteLine("6. feladat:");
        Console.Write("Kérem, adja meg a rendszámot: ");
        string kertRendszam = Console.ReadLine()!.Trim();

        var autóJeladások = adatok
            .Where(j => j.Rendszam == kertRendszam)
            .ToList();

        if (autóJeladások.Count == 0)
        {
            Console.WriteLine("Nincs ilyen rendszámú jármű az adatokban.");
        }
        else
        {
            double tavolsag = 0.0;
            for (int i = 0; i < autóJeladások.Count; i++)
            {
                var j = autóJeladások[i];
                Console.WriteLine($"{j.Ora}:{j.Perc} {tavolsag:F1} km");

                if (i + 1 < autóJeladások.Count)
                {
                    var kov = autóJeladások[i + 1];
                    double elteltOra =
                        (kov.Ora * 60 + kov.Perc - j.Ora * 60 - j.Perc) / 60.0;
                    tavolsag += j.Sebesseg * elteltOra;
                }
            }
        }
        Console.WriteLine();

        // ------------------------------------------------------------------
        // 7. FELADAT – ido.txt létrehozása
        // ------------------------------------------------------------------
        // Soronként: rendszám  első_jeladás_óra  perc  utolsó_jeladás_óra  perc
        // Minden jármű pontosan egyszer szerepel (az adatok időrendben vannak).
        var jarmuSorrendben = new List<string>();
        var elsoJeladas     = new Dictionary<string, (int ora, int perc)>();
        var utolsoJeladas   = new Dictionary<string, (int ora, int perc)>();

        foreach (var j in adatok)
        {
            if (!elsoJeladas.ContainsKey(j.Rendszam))
            {
                jarmuSorrendben.Add(j.Rendszam);
                elsoJeladas[j.Rendszam] = (j.Ora, j.Perc);
            }
            utolsoJeladas[j.Rendszam] = (j.Ora, j.Perc);
        }

        using (var fw = new StreamWriter("ido.txt"))
        {
            foreach (string r in jarmuSorrendben)
            {
                var (fo, fp) = elsoJeladas[r];
                var (lo, lp) = utolsoJeladas[r];
                fw.WriteLine($"{r} {fo} {fp} {lo} {lp}");
            }
        }

        Console.WriteLine("7. feladat:");
        Console.WriteLine("Az ido.txt állomány sikeresen elkészült.");
    }
}