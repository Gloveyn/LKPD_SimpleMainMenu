using System;

namespace KerangkaGame
{
    class Karakter
    {
        public int totalSenjata, kekuatan;

        public string nama { get; private set; }
        public int kesehatan { get; private set; }
        public int senjata { get; private set; }
        public int posisi { get; private set; } = 0;

        private int kesehatanAwal; // Menyimpan HP awal supaya bisa dipulihkan saat permainan diulang

        public Karakter(string nama, int kesehatan, int senjata)   // membuat constructor
        {
            this.nama = nama;
            this.kesehatan = kesehatan;
            this.kesehatanAwal = kesehatan;
            this.senjata = senjata;
            this.posisi = 0;
        }

        // Method menampilkan nama karakter
        public void tampilkan()
        {
            Console.WriteLine($"Karaktermu : {nama}");
        }

        public void Serang(Karakter target)
        {
            Console.WriteLine($"===> {nama} menyerang {target.nama}");
            target.TerimaSerangan(this.senjata);     // Panggil method di bawahnya
        }

        // Serang dengan damage yang ditentukan sendiri oleh player
        public void Serang(Karakter target, int damage)
        {
            Console.WriteLine($"===> {nama} menyerang {target.nama}");
            target.TerimaSerangan(damage);
        }

        public void TerimaSerangan(int jumlahSerangan) // objek menerima serangan
        {
            kesehatan -= jumlahSerangan;
            if (kesehatan < 0)
            {
                kesehatan = 0; // Kesehatan tidak boleh minus
            }
            Console.WriteLine($"{nama} diserang dengan {jumlahSerangan}. Sisa kesehatan {kesehatan}");
        }

        // Mengembalikan kesehatan ke nilai awal (dipakai setiap permainan baru)
        public void PulihkanPenuh()
        {
            kesehatan = kesehatanAwal;
        }

        // Perpindahan posisi (koordinat) dengan memasukkan angka
        public void Geser(int langkah)
        {
            int posisiBaru = posisi + langkah;

            if (posisiBaru < 0)
            {
                Console.WriteLine("Sudah tidak dapat berpindah (Posisi tidak boleh kurang dari 0).");
            }
            else
            {
                posisi = posisiBaru;
                Console.WriteLine($"{nama} berpindah {langkah}. Posisi saat ini: {posisi}");
            }
        }

        // Method PindahPosisi interaktif menggunakan tombol D, A, dan Esc
        public void PindahPosisi()
        {
            while (true)
            {
                // Membaca tombol keyboard tanpa harus menekan Enter
                ConsoleKeyInfo tombol = Console.ReadKey(true);

                if (tombol.Key == ConsoleKey.D)
                {
                    posisi += 2; // Bertambah 2
                    Console.WriteLine($"Posisi saat ini: {posisi}");
                }
                else if (tombol.Key == ConsoleKey.A)
                {
                    if (posisi > 0)
                    {
                        posisi -= 1; // Berkurang 1
                        Console.WriteLine($"Posisi saat ini: {posisi}");
                    }
                    else
                    {
                        Console.WriteLine("Sudah tidak dapat berpindah (Posisi sudah 0).");
                    }
                }
                else if (tombol.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("Selesai menggunakan tombol keyboard.");
                    break; // Keluar dari perulangan
                }
            }
        }

        public void getData()
        {
            Console.WriteLine($"Karakter {nama}, HP: {kesehatan}, Posisi: {posisi}");
        }
    }

    class MainProgram
    {
        // Karakter dibuat sekali di sini supaya datanya (HP, posisi) tetap tersimpan
        // selama menu diulang-ulang.
        static Karakter player1 = new Karakter("Alucard", 100, 30);   // Main player
        static Karakter musuh = new Karakter("Balmond", 100, 20);

        static void Main(string[] args)
        {
            bool statusGame = true;

            while (statusGame)
            {
                Console.Clear(); // Membersihkan layar agar menu lebih rapi
                Console.WriteLine("===== Menu Game =====");
                Console.WriteLine("1. Informasi Karakter");
                Console.WriteLine("2. Mainkan Permainan");
                Console.WriteLine("3. Perpindahan Angka");
                Console.WriteLine("4. Tombol Keyboard");
                Console.WriteLine("5. Keluar");
                Console.Write("Pilihanmu : ");

                // input menggunakan ReadLine()
                string menuPilihan = Console.ReadLine() ?? "";
                Console.WriteLine();

                switch (menuPilihan)
                {
                    case "1":
                        InformasiKarakter();
                        break;

                    case "2":
                        MainkanPermainan();
                        break;

                    case "3":
                        PerpindahanAngka();
                        break;

                    case "4":
                        TombolKeyboard();
                        break;

                    case "5":
                        Console.WriteLine("Keluar permainan");
                        statusGame = false;
                        break;

                    default:
                        Console.WriteLine("Pilihan tidak valid.");
                        break;
                }

                if (statusGame)
                {
                    Console.WriteLine();
                    Console.WriteLine("Tekan tombol apa saja untuk kembali ke menu... ");
                    Console.ReadKey(true); // Menunggu tombol ditekan
                }
            }
        }

        // 1.Menampilkan lebih
        static void InformasiKarakter()
        {
            Console.WriteLine("Karakter Tersedia : ");
            player1.tampilkan();
            player1.getData();

            Console.WriteLine();
            Console.WriteLine("Musuh : ");
            musuh.getData();
        }

        // opsi 2 permainan (bisa menyerang)
        static void MainkanPermainan()
        {
            
            musuh.PulihkanPenuh();

            Console.WriteLine("===== Mainkan Permainan =====");
            Console.WriteLine($"{player1.nama} VS {musuh.nama}");
            Console.WriteLine("Masukkan damage untuk menyerang. Ketik 0 untuk berhenti.");

            while (musuh.kesehatan > 0)
            {
                Console.WriteLine();
                musuh.getData();
                Console.Write("Damage : ");

                if (!int.TryParse(Console.ReadLine(), out int damage) || damage < 0)
                {
                    Console.WriteLine("Input harus berupa angka bulat positif.");
                    continue;
                }

                if (damage == 0)
                {
                    Console.WriteLine($"{player1.nama} berhenti bertarung.");
                    break;
                }

                player1.Serang(musuh, damage);
            }

            if (musuh.kesehatan <= 0)
            {
                Console.WriteLine($"\n{musuh.nama} kalah! {player1.nama} menang.");
            }
        }

        // Perpindahan karakter menggunakan angka
        static void PerpindahanAngka()
        {
            while (true)
            {
                Console.WriteLine("===== Perpindahan Angka =====");
                player1.getData();
                Console.Write("Masukkan jumlah langkah (angka negatif untuk mundur): ");

                if (int.TryParse(Console.ReadLine(), out int langkah))
                {
                    if (langkah == 0)
                    {
                        Console.WriteLine("Selesai Berpindah");
                        break;
                    }

                    player1.Geser(langkah);
                }
                else
                {
                    Console.WriteLine("Input harus berupa angka.");
                }

                player1.getData();
            }
        }

        // perpindahan karakter menggunakan tombol keyboard
        static void TombolKeyboard()
        {
            Console.WriteLine("===== Tombol Keyboard =====");
            Console.WriteLine("Tekan D untuk maju (+2), A untuk mundur (-1), Esc untuk selesai.");
            player1.PindahPosisi();
        }
    }
}