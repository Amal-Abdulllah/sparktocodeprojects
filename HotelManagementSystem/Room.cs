using System;

namespace HotelManagementSystem
{
    // Represents one hotel room
    public class Room
    {
        public int RoomNumber;          // unique room number, e.g. 101
        public string RoomType;         // "Single", "Double", or "Suite"
        public double PricePerNight;    // nightly rate in OMR
        public bool IsAvailable;        // true = free, false = booked

        // Constructor: a new room is always available when first added
        public Room(int roomNumber, string roomType, double pricePerNight)
        {
            RoomNumber = roomNumber;
            RoomType = roomType;
            PricePerNight = pricePerNight;
            IsAvailable = true;
        }

        // Prints one formatted line describing this room
        public void DisplayRoom()
        {
            string status = IsAvailable ? "Available" : "Booked";
            Console.WriteLine("Room {0,-5} | {1,-7} | OMR {2,8:F2}/night | {3}",
                RoomNumber, RoomType, PricePerNight, status);
        }
    }
}