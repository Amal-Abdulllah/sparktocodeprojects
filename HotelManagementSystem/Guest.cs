using System;
using System.Collections.Generic;
using System.Linq;

namespace HotelManagementSystem
{
    // Represents one registered guest
    public class Guest
    {
        public string GuestId;      // auto-generated, e.g. "G001"
        public string GuestName;
        public string RoomNumber;   // "Not Assigned" until a room is booked
        public string CheckInDate;  // stored as a plain string, e.g. "15/07/2026"
        public int TotalNights;

        public Guest(string guestId, string guestName, string checkInDate, int totalNights)
        {
            GuestId = guestId;
            GuestName = guestName;
            RoomNumber = "Not Assigned";   // default: no room yet (Case 02)
            CheckInDate = checkInDate;
            TotalNights = totalNights;
        }

        // Prints one formatted line describing this guest
        public void DisplayGuest()
        {
            Console.WriteLine("{0} | {1,-15} | Room: {2,-12} | Check-in: {3} | {4} night(s)",
                GuestId, GuestName, RoomNumber, CheckInDate, TotalNights);
        }

        // Total cost = nights x price of the guest's room.
        // The room's price lives in the rooms list, so we receive that list,
        // find the matching room with LINQ, and multiply.
        public double CalculateTotalCost(List<Room> rooms)
        {
            if (RoomNumber == "Not Assigned")
                return 0;

            Room myRoom = rooms.FirstOrDefault(r => r.RoomNumber.ToString() == RoomNumber);
            if (myRoom == null)
                return 0;

            return TotalNights * myRoom.PricePerNight;
        }
    }
}