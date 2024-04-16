public void UpdateReservation(int numOfPeople, string name, string addition, string surname, int phoneNumber, string email, DateTime date)
{
    string connectionString = "your_connection_string_here";
    using (SqlConnection connection = new SqlConnection(connectionString))
    {
        string query = "UPDATE Reservations SET Date = @ReservationDate, NumOfPeople = @NumOfPeople, FirstName = @FirstName, Addition = @Addition, LastName = @LastName, PhoneNumber = @PhoneNumber, Email = @Email WHERE ReservationId = @ReservationId";
        SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@ReservationDate", date);
        command.Parameters.AddWithValue("@NumOfPeople", numOfPeople);
        command.Parameters.AddWithValue("@FirstName", name);
        command.Parameters.AddWithValue("@Addition", addition);
        command.Parameters.AddWithValue("@LastName", surname);
        command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
        command.Parameters.AddWithValue("@Email", email);
        command.Parameters.AddWithValue("@ReservationId", reservationId);

        try
        {
            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();
            if (rowsAffected > 0)
            {
                Console.WriteLine("Reservation updated successfully.");
            }
            else
            {
                Console.WriteLine("No reservation found with the provided ID.");
            }
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error updating reservation: " + ex.Message);
        }
    }
}

public void ChangeReservation()
{

    bool Change_checker = false;
    while (!Change_checker)
    {
        bool Change_date_checker = false;
        bool Change_first_name_checker = false;
        bool Change_last_name_checker = false;

        string Change_date = "";
        string Change_name = "";
        string Change_surname = "";

        while (!Change_date_checker)
        {
            Console.Write("Voer uw reserveringsdatum in (dd-MM-yyyy): ");
            Change_date = Console.ReadLine() ?? "";
            DateTime parsedDate;
            if (DateTime.TryParseExact(Change_date, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDate))
            {
                Change_date_checker = true;
            }
            else
            {
                Console.WriteLine("Incorrect formaat. Probeer: (dd-MM-yyyy)");
            }
        }

        while (!Change_first_name_checker)
        {
            Console.Write("Voornaam: ");
            Change_name = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(Change_name))
            {
                Change_first_name_checker = true;
            }
            else
            {
                Console.WriteLine("Ongeldige invoer. Probeer alleen letters te gebruiken.");
            }
        }

        while (!Change_last_name_checker)
        {
            Console.Write("Achternaam: ");
            Change_surname = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(Change_surname))
            {
                Change_last_name_checker = true;
            }
            else
            {
                Console.WriteLine("Ongeldige invoer. Probeer alleen letters te gebruiken.");
            }
        }
        
        try
        {
            @"SELECT * FROM Reserveringen WHERE First_name = @FirstName AND Last_name = @LastName AND Date = @Date";
        }
        catch (SqlException ex)
        {
            Console.WriteLine("Error updating reservation: " + ex.Message);
            Exit();
        }

        Console.WriteLine("\nReserveringsgegevens:");
        Console.WriteLine("Datum: " + Change_date);
        Console.WriteLine("Voornaam: " + Change_name);
        Console.WriteLine("Achternaam: " + Change_surname);
        Console.WriteLine("Weet u zeker dat u deze reservering wilt veranderen? (ja/nee)");
        string CRD_confirmation = Console.ReadLine()?.Trim().ToLower();
        if (CRD_confirmation == "ja")
        {

            Change_checker = true;
        }
        else
        {
            Exit();
        }
    }

    bool CRC_checker = false;
    while (CRC_checker == false)
    {
        bool CRC_date_checker = false;
        bool CRC_first_name_checker = false;
        bool CRC_last_name_checker = false;
        bool CRC_phoneNumber_checker = false;
        bool CRC_people_checker = false;
        bool CRC_mail_checker = false;

        string CRC_date = "";
        int CRC_numOfPeople = 0;
        string CRC_name = "";
        string CRC_surname = "";
        string CRC_phoneNumber = "";
        string CRC_email = "";
        string CRC_addition = "";
        string CRC_reservation_checker = "";

        while (CRC_date_checker != true)
        {
            Console.WriteLine("Welkom bij het reserveringsapplicatie van YES!");
            Console.Write("Voer uw reserveringsdatum in (dd-MM-yyyy): ");
            CRC_date = Console.ReadLine() ?? "";
            DateTime parsedDate;
            if (DateTime.TryParseExact(CRC_date, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out parsedDate))
            {
                CRC_date_checker = true;
            }
            else
            {
                Console.WriteLine("Incorrecte formaat. Probeer: (dd-MM-yyyy)");
            }
        }

        Console.WriteLine("Wilt u aan het raam? (ja/nee):");
        bool CRC_wantWindow = Console.ReadLine()?.Trim().ToLower() == "ja";

        while (CRC_people_checker != true)
        {
            Console.Write("Aantal personen: ");
            if (int.TryParse(Console.ReadLine(), out CRC_numOfPeople) && CRC_numOfPeople > 0 && CRC_numOfPeople < 48)
            {
                CRC_people_checker = true;
            }
            else
            {
                Console.WriteLine("Invalid aantal personen. Het aantal personen moet tussen 1 en 48 zijn.");
            }
        }

        ReservationSystem rs = new ReservationSystem();
        rs.ReserveTableForGroup(CRC_numOfPeople, CRC_wantWindow);

        while (!CRC_first_name_checker)
        {
            Console.WriteLine("\nGraag uw contactgegevens achterlaten:");
            Console.Write("Voornaam: ");
            CRC_name = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(CRC_name))
            {
                CRC_first_name_checker = true;
            }
            else
            {
                Console.WriteLine("Invalid input. Probeer alleen letters te gebruiken.");
            }
        }

        Console.Write("Toevoeging: ");
        CRC_addition = Console.ReadLine() ?? "";

        while (!CRC_last_name_checker)
        {
            Console.Write("Achternaam: ");
            CRC_surname = Console.ReadLine() ?? "";
            if (!string.IsNullOrWhiteSpace(CRC_surname))
            {
                CRC_last_name_checker = true;
            }
            else
            {
                Console.WriteLine("Invalid input. Probeer alleen letters te gebruiken.");
            }
        }

        while (!CRC_phoneNumber_checker)
        {
            Console.Write("Telefoonnummer: ");
            CRC_phoneNumber = Console.ReadLine() ?? "";
            if (CRC_phoneNumber.Length == 10 && long.TryParse(CRC_phoneNumber, out _))
            {
                CRC_phoneNumber_checker = true;
            }
            else
            {
                Console.WriteLine("Telefoonnummer moet 10 cijfers lang zijn.");
            }
        }

        while (!CRC_mail_checker)
        {
            Console.Write("E-mail: ");
            CRC_email = Console.ReadLine() ?? "";
            if (CRC_email.Contains("@") && CRC_email.Contains("."))
            {
                CRC_mail_checker = true;
            }
            else
            {
                Console.WriteLine("Invalid email. Probeer een echt email in te vullen.");
            }
        }

        Console.WriteLine("\nHeeft u nog opmerkingen of verzoeken?");
        string CRC_comments = Console.ReadLine() ?? "";

        DateTime CRC_reservationDate;
        if (DateTime.TryParseExact(CRC_date, "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out CRC_reservationDate))
        {
            db.AddReservation(CRC_numOfPeople, CRC_name, CRC_addition, CRC_surname, int.Parse(CRC_phoneNumber), CRC_email, CRC_reservationDate);
        }

        Console.WriteLine("\nReserveringsgegevens:");
        Console.WriteLine("Datum: " + CRC_date);
        Console.WriteLine("Aantal Personen: " + CRC_numOfPeople);
        Console.WriteLine("Voornaam: " + CRC_name);
        Console.WriteLine("Toevoering: " + CRC_addition);
        Console.WriteLine("Achternaam: " + CRC_surname);
        Console.WriteLine("Telefoonnummer: " + CRC_phoneNumber);
        Console.WriteLine("E-mail: " + CRC_email);
        Console.WriteLine("Opmerkingen: " + CRC_comments);
        Console.WriteLine("Is dit je gewenste reservatie? (ja/nee)");
        string CRC_confirmation = Console.ReadLine()?.Trim().ToLower() == "ja";
        if (CRC_confirmation == "ja")
        {
            // Attempt to change the reservation
            if (db.UpdateReservation(CRC_date, CRC_numOfPeople, CRC_name, CRC_addition, CRC_surname, CRC_phoneNumber, CRC_email, CRC_comments))
            {
                CRC_checker = true;
                Console.WriteLine("\nReservering succesvol veranderd!");
            }
            else
            {
                Console.WriteLine("\nReservering niet gevonden of kon niet worden veranderd.");
            }
        }
        else
        {
            CRC_checker = true; // Exit loop if not confirmed to delete
        }
    }

    Console.WriteLine("\nDank u wel! We hopen u gauw te zien bij YES!");
}