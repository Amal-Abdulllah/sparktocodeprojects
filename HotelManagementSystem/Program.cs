namespace HotelManagementSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // The two system lists
            List<Room> rooms = new List<Room>();
            List<Guest> guests = new List<Guest>();

            // Pre-load 6 rooms of mixed types
            rooms.Add(new Room(101, "Single", 25.00));
            rooms.Add(new Room(102, "Single", 28.50));
            rooms.Add(new Room(201, "Double", 45.00));
            rooms.Add(new Room(202, "Double", 48.75));
            rooms.Add(new Room(301, "Suite", 90.00));
            rooms.Add(new Room(302, "Suite", 120.00));

            bool running = true;
            while (running)
            {
                Console.WriteLine();
                Console.WriteLine("Grand Vista Hotel - Management System");
                Console.WriteLine();
                Console.WriteLine(" 1. Add New Room");
                Console.WriteLine(" 2. Register New Guest");
                Console.WriteLine(" 3. Book a Room for a Guest");
                Console.WriteLine(" 4. View All Rooms");
                Console.WriteLine(" 5. View All Guests");
                Console.WriteLine(" 6. Search & Filter Rooms");
                Console.WriteLine(" 7. Guest & Booking Statistics");
                Console.WriteLine(" 8. Update Room Price");
                Console.WriteLine(" 9. Guest Lookup by Name");
                Console.WriteLine("10. Room Type Breakdown Report");
                Console.WriteLine("11. Check Out a Guest");
                Console.WriteLine("12. Remove Unavailable Rooms");
                Console.WriteLine("13. Extend Guest Stay");
                Console.WriteLine("14. Highest Revenue Booking");
                Console.WriteLine("15. Guest Pagination Viewer");
                Console.WriteLine(" 0. Exit");
                Console.WriteLine();
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": AddNewRoom(rooms); break;
                    case "2": RegisterNewGuest(guests); break;
                    case "3": BookRoom(rooms, guests); break;
                    case "4": ViewAllRooms(rooms); break;
                    case "5": ViewAllGuests(guests); break;
                    case "6": SearchAndFilterRooms(rooms); break;
                    case "7": GuestBookingStatistics(rooms, guests); break;
                    case "8": UpdateRoomPrice(rooms); break;
                    case "9": GuestLookupByName(guests); break;
                    case "10": RoomTypeBreakdown(rooms); break;
                    case "11": CheckOutGuest(rooms, guests); break;
                    case "12": RemoveUnavailableRooms(rooms, guests); break;
                    case "13": ExtendGuestStay(rooms, guests); break;
                    case "14": HighestRevenueBooking(rooms, guests); break;
                    case "15": GuestPaginationViewer(guests); break;

                    case "0":
                        Console.WriteLine("Goodbye!");
                        running = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please enter 0-15.");
                        break;
                }
            }
        }

        // Case methods go below this line

        // Case 01 - Add New Room
        static void AddNewRoom(List<Room> rooms)
        {
            Console.WriteLine(" Add New Room");

            Console.Write("Enter room number: ");
            int roomNumber;
            if (!int.TryParse(Console.ReadLine(), out roomNumber) || roomNumber <= 0)
            {
                Console.WriteLine("Error: room number must be a positive number.");
                return;
            }

            // LINQ Any(): does any existing room already use this number?
            if (rooms.Any(r => r.RoomNumber == roomNumber))
            {
                Console.WriteLine("Error: room {0} already exists.", roomNumber);
                return;
            }

            Console.Write("Enter room type (Single / Double / Suite): ");
            string roomType = Console.ReadLine();

            Console.Write("Enter price per night: ");
            double price;
            if (!double.TryParse(Console.ReadLine(), out price) || price <= 0)
            {
                Console.WriteLine("Error: price must be a positive number.");
                return;
            }

            rooms.Add(new Room(roomNumber, roomType, price));

            Console.WriteLine();
            Console.WriteLine("Room added successfully!");
            Console.WriteLine("Number: {0} | Type: {1} | Price: OMR {2:F2}/night | Status: Available",
                roomNumber, roomType, price);
            Console.WriteLine("Total rooms in system: {0}", rooms.Count);
        }

        // Case 02 - Register New Guest
        static void RegisterNewGuest(List<Guest> guests)
        {
            Console.WriteLine(" Register New Guest");

            Console.Write("Enter guest name: ");
            string name = Console.ReadLine();

            Console.Write("Enter check-in date (e.g. 15/07/2026): ");
            string checkInDate = Console.ReadLine();

            Console.Write("Enter number of nights: ");
            int nights;
            if (!int.TryParse(Console.ReadLine(), out nights) || nights <= 0)
            {
                Console.WriteLine("Error: number of nights must be a positive integer.");
                return;
            }

            // Auto-generate ID from the current list size: G001, G002, ...
            string guestId = "G" + (guests.Count + 1).ToString("D3");

            guests.Add(new Guest(guestId, name, checkInDate, nights));

            Console.WriteLine();
            Console.WriteLine("Guest registered successfully!");
            Console.WriteLine("ID: {0} | Name: {1} | Check-in: {2} | Nights: {3} | Room: Not Assigned",
                guestId, name, checkInDate, nights);
        }

        // Case 03 - Book a Room for a Guest
        static void BookRoom(List<Room> rooms, List<Guest> guests)
        {
            Console.WriteLine("Book a Room for a Guest");

            Console.Write("Enter guest ID (e.g. G001): ");
            string guestId = Console.ReadLine();

            // LINQ FirstOrDefault(): returns the guest, or null if not found
            Guest guest = guests.FirstOrDefault(g => g.GuestId == guestId);
            if (guest == null)
            {
                Console.WriteLine("Error: guest {0} not found.", guestId);
                return;
            }

            Console.Write("Enter desired room number: ");
            int roomNumber;
            if (!int.TryParse(Console.ReadLine(), out roomNumber))
            {
                Console.WriteLine("Error: invalid room number.");
                return;
            }

            Room room = rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
            if (room == null)
            {
                Console.WriteLine("Error: room {0} not found.", roomNumber);
                return;
            }

            if (!room.IsAvailable)
            {
                Console.WriteLine("Room is already booked.");
                return;
            }

            // Link the two objects: update both in place
            guest.RoomNumber = room.RoomNumber.ToString();
            room.IsAvailable = false;

            Console.WriteLine();
            Console.WriteLine("Booking confirmed!");
            Console.WriteLine("Guest:           {0}", guest.GuestName);
            Console.WriteLine("Room:            {0} ({1})", room.RoomNumber, room.RoomType);
            Console.WriteLine("Price per night: OMR {0:F2}", room.PricePerNight);
            Console.WriteLine("Total nights:    {0}", guest.TotalNights);
            Console.WriteLine("Total cost:      OMR {0:F2}", guest.CalculateTotalCost(rooms));
        }

        // Case 04 - View All Rooms
        static void ViewAllRooms(List<Room> rooms)
        {
            Console.WriteLine("--- All Rooms ---");

            if (rooms.Count == 0)
            {
                Console.WriteLine("No rooms have been added yet.");
                return;
            }

            Console.WriteLine("Total rooms: {0}", rooms.Count());
            Console.WriteLine();

            // OrderBy() sorts by room number ascending without touching the list
            foreach (Room r in rooms.OrderBy(r => r.RoomNumber))
            {
                r.DisplayRoom();
            }
        }

        // Case 05 - View All Guests
        static void ViewAllGuests(List<Guest> guests)
        {
            Console.WriteLine("All Guests");

            if (guests.Count == 0)
            {
                Console.WriteLine("No guests have been registered yet.");
                return;
            }

            Console.WriteLine("Total guests: {0}", guests.Count());
            Console.WriteLine();

            foreach (Guest g in guests.OrderBy(g => g.GuestName))
            {
                g.DisplayGuest();
            }
        }

        // Case 06 - Search & Filter Rooms
        static void SearchAndFilterRooms(List<Room> rooms)
        {
            Console.WriteLine("Search & Filter Rooms");
            Console.WriteLine("1. Show all available rooms");
            Console.WriteLine("2. Filter by room type");
            Console.WriteLine("3. Filter by max price");
            Console.WriteLine("4. Room price statistics");
            Console.WriteLine("0. Back");
            Console.Write("Enter option: ");
            string option = Console.ReadLine();
            Console.WriteLine();

            if (option == "1")
            {
                // Available rooms, cheapest first
                var available = rooms.Where(r => r.IsAvailable)
                                     .OrderBy(r => r.PricePerNight);

                Console.WriteLine("Available rooms found: {0}", available.Count());
                if (!available.Any())
                {
                    Console.WriteLine("No rooms found for the selected criteria.");
                    return;
                }
                foreach (Room r in available) r.DisplayRoom();
            }
            else if (option == "2")
            {
                Console.Write("Enter room type (Single / Double / Suite): ");
                string type = Console.ReadLine();

                var byType = rooms.Where(r => r.RoomType.ToLower() == type.ToLower());

                Console.WriteLine("Rooms of type '{0}': {1}", type, byType.Count());
                if (!byType.Any())
                {
                    Console.WriteLine("No rooms found for the selected criteria.");
                    return;
                }
                foreach (Room r in byType) r.DisplayRoom();
            }
            else if (option == "3")
            {
                Console.Write("Enter maximum price: ");
                double maxPrice;
                if (!double.TryParse(Console.ReadLine(), out maxPrice) || maxPrice <= 0)
                {
                    Console.WriteLine("Error: price must be a positive number.");
                    return;
                }

                var affordable = rooms.Where(r => r.IsAvailable && r.PricePerNight <= maxPrice)
                                      .OrderBy(r => r.PricePerNight);

                Console.WriteLine("Available rooms at or below OMR {0:F2}: {1}", maxPrice, affordable.Count());
                if (!affordable.Any())
                {
                    Console.WriteLine("No rooms found for the selected criteria.");
                    return;
                }
                foreach (Room r in affordable) r.DisplayRoom();
            }
            else if (option == "4")
            {
                if (rooms.Count == 0)
                {
                    Console.WriteLine("No rooms found for the selected criteria.");
                    return;
                }

                Console.WriteLine("Total rooms:          {0}", rooms.Count());
                Console.WriteLine("Available rooms:      {0}", rooms.Count(r => r.IsAvailable));
                Console.WriteLine("Average price:        OMR {0:F2}", rooms.Average(r => r.PricePerNight));
                Console.WriteLine("Cheapest price:       OMR {0:F2}", rooms.Min(r => r.PricePerNight));
                Console.WriteLine("Most expensive price: OMR {0:F2}", rooms.Max(r => r.PricePerNight));
            }
            else if (option != "0")
            {
                Console.WriteLine("Invalid option.");
            }
        }

        // Case 07 - Guest & Booking Statistics
        static void GuestBookingStatistics(List<Room> rooms, List<Guest> guests)
        {
            Console.WriteLine(" Guest & Booking Statistics ");

            // Guest counts
            Console.WriteLine("Total registered guests:   {0}", guests.Count());
            Console.WriteLine("Guests with a room booked: {0}",
                guests.Count(g => g.RoomNumber != "Not Assigned"));

            // Room counts
            Console.WriteLine("Total rooms:               {0}", rooms.Count());
            Console.WriteLine("Currently booked rooms:    {0}", rooms.Count(r => !r.IsAvailable));

            // Guests that actually hold a booking
            var activeGuests = guests.Where(g => g.RoomNumber != "Not Assigned");

            if (!activeGuests.Any())
            {
                Console.WriteLine("No active bookings recorded.");
                return;
            }

            // Average nights of active bookings
            Console.WriteLine("Average nights (active):   {0:F1}",
                activeGuests.Average(g => g.TotalNights));

            // Top 3 highest-spending guests
            Console.WriteLine();
            Console.WriteLine("Top 3 highest-spending guests:");
            var top3 = activeGuests.OrderByDescending(g => g.CalculateTotalCost(rooms))
                                   .Take(3);
            foreach (Guest g in top3)
            {
                Console.WriteLine("  {0} - Room {1} - OMR {2:F2}",
                    g.GuestName, g.RoomNumber, g.CalculateTotalCost(rooms));
            }

            // Select(): project each active guest into a summary string
            Console.WriteLine();
            Console.WriteLine("Booking summary:");
            var summaries = activeGuests.Select(g =>
                string.Format("{0} - Room {1} - {2} nights - OMR {3:F2}",
                    g.GuestName, g.RoomNumber, g.TotalNights, g.CalculateTotalCost(rooms)));
            foreach (string line in summaries)
            {
                Console.WriteLine("  " + line);
            }
        }

        // Case 08 - Update Room Price
        static void UpdateRoomPrice(List<Room> rooms)
        {
            Console.WriteLine("Update Room Price");

            Console.Write("Enter room number: ");
            int roomNumber;
            if (!int.TryParse(Console.ReadLine(), out roomNumber))
            {
                Console.WriteLine("Error: invalid room number.");
                return;
            }

            Room room = rooms.FirstOrDefault(r => r.RoomNumber == roomNumber);
            if (room == null)
            {
                Console.WriteLine("Error: room {0} not found.", roomNumber);
                return;
            }

            Console.Write("Enter new price per night: ");
            double newPrice;
            if (!double.TryParse(Console.ReadLine(), out newPrice) || newPrice <= 0)
            {
                Console.WriteLine("Error: price must be a positive number. No change made.");
                return;
            }

            double oldPrice = room.PricePerNight;
            room.PricePerNight = newPrice;   // updated in place

            Console.WriteLine("Price updated for room {0}: OMR {1:F2} -> OMR {2:F2}",
                room.RoomNumber, oldPrice, newPrice);
        }

        // Case 09 - Guest Lookup by Name
        static void GuestLookupByName(List<Guest> guests)
        {
            Console.WriteLine(" Guest Lookup by Name");

            Console.Write("Enter name or part of a name: ");
            string search = Console.ReadLine();

            // Case-insensitive "contains" search
            var matches = guests.Where(g =>
                g.GuestName.ToLower().Contains(search.ToLower()));

            if (!matches.Any())
            {
                Console.WriteLine("No guests matched that search.");
                return;
            }

            Console.WriteLine("Matches found: {0}", matches.Count());
            foreach (Guest g in matches)
            {
                Console.WriteLine("  {0} | {1} | Room: {2}", g.GuestId, g.GuestName, g.RoomNumber);
            }
        }

        // Case 10 - Room Type Breakdown Report
        static void RoomTypeBreakdown(List<Room> rooms)
        {
            Console.WriteLine(" Room Type Breakdown Report ");

            string[] types = { "Single", "Double", "Suite" };

            foreach (string type in types)
            {
                int count = rooms.Count(r => r.RoomType == type);
                Console.Write("{0,-7} | Rooms: {1,2} | Avg price: ", type, count);

                if (count == 0)
                {
                    // Average() on an empty set throws an error, so show N/A instead
                    Console.WriteLine("N/A");
                }
                else
                {
                    Console.WriteLine("OMR {0:F2}",
                        rooms.Where(r => r.RoomType == type).Average(r => r.PricePerNight));
                }
            }

            Console.WriteLine();
            if (rooms.Count > 0)
            {
                Console.WriteLine("Overall average price: OMR {0:F2}",
                    rooms.Average(r => r.PricePerNight));
            }
            else
            {
                Console.WriteLine("Overall average price: N/A");
            }
        }

        // Case 11 - Check Out a Guest
        static void CheckOutGuest(List<Room> rooms, List<Guest> guests)
        {
            Console.WriteLine("Check Out a Guest ");

            Console.Write("Enter guest ID to check out: ");
            string guestId = Console.ReadLine();

            Guest guest = guests.FirstOrDefault(g => g.GuestId == guestId);
            if (guest == null)
            {
                Console.WriteLine("Error: guest {0} not found.", guestId);
                return;
            }

            if (guest.RoomNumber == "Not Assigned")
            {
                Console.WriteLine("This guest has no active booking.");
                return;
            }

            // Linked lookup: use the guest's room number to find the Room object
            Room room = rooms.FirstOrDefault(r => r.RoomNumber.ToString() == guest.RoomNumber);
            if (room == null)
            {
                Console.WriteLine("Error: room {0} not found in the system.", guest.RoomNumber);
                return;
            }

            // Final bill
            Console.WriteLine();
            Console.WriteLine("Final Bill");
            Console.WriteLine("Guest:           {0}", guest.GuestName);
            Console.WriteLine("Room:            {0} ({1})", room.RoomNumber, room.RoomType);
            Console.WriteLine("Check-in date:   {0}", guest.CheckInDate);
            Console.WriteLine("Total nights:    {0}", guest.TotalNights);
            Console.WriteLine("Price per night: OMR {0:F2}", room.PricePerNight);
            Console.WriteLine("Total cost:      OMR {0:F2}", guest.CalculateTotalCost(rooms));
            Console.WriteLine();

            Console.Write("Confirm checkout? (Y/N): ");
            string confirm = Console.ReadLine();

            if (confirm.ToUpper() != "Y")
            {
                Console.WriteLine("Checkout cancelled. No changes made.");
                return;
            }

            // Order matters: free the room FIRST, then remove the guest
            room.IsAvailable = true;
            guests.Remove(guest);

            Console.WriteLine();
            Console.WriteLine("Checkout complete!");
            Console.WriteLine("Remaining guests: {0} | Total rooms: {1}", guests.Count, rooms.Count);

            // Any(): confirm the room is now available again
            bool roomFree = rooms.Any(r => r.RoomNumber == room.RoomNumber && r.IsAvailable);
            Console.WriteLine("Room {0} available again: {1}", room.RoomNumber, roomFree ? "Yes" : "No");
        }

        // Case 12 - Remove Unavailable Rooms
        static void RemoveUnavailableRooms(List<Room> rooms, List<Guest> guests)
        {
            Console.WriteLine("Remove Unavailable Rooms");

            // Removable = unavailable AND no guest currently holds that number
            var removable = rooms.Where(r => !r.IsAvailable &&
                                !guests.Any(g => g.RoomNumber == r.RoomNumber.ToString()))
                                 .OrderBy(r => r.RoomNumber);

            if (!removable.Any())
            {
                Console.WriteLine("All unavailable rooms are currently occupied. No rooms can be decommissioned.");
                return;
            }

            Console.WriteLine("Safely removable rooms:");
            foreach (Room r in removable)
            {
                Console.WriteLine("  Room {0} | {1} | OMR {2:F2}", r.RoomNumber, r.RoomType, r.PricePerNight);
            }
            Console.WriteLine("Count: {0}", removable.Count());

            Console.Write("Confirm removal? (Y/N): ");
            string confirm = Console.ReadLine();

            if (confirm.ToUpper() != "Y")
            {
                Console.WriteLine("Removal cancelled. No rooms were removed.");
                return;
            }

            // RemoveAll() with the SAME condition as the preview above
            rooms.RemoveAll(r => !r.IsAvailable &&
                !guests.Any(g => g.RoomNumber == r.RoomNumber.ToString()));

            Console.WriteLine();
            Console.WriteLine("Rooms removed. Updated total room count: {0}", rooms.Count);

            // Select(): project remaining rooms into "number - type" strings
            Console.WriteLine("Remaining rooms:");
            var remaining = rooms.Select(r => r.RoomNumber + " - " + r.RoomType);
            foreach (string line in remaining)
            {
                Console.WriteLine("  " + line);
            }
        }

        // Case 13 - Extend Guest Stay
        static void ExtendGuestStay(List<Room> rooms, List<Guest> guests)
        {
            Console.WriteLine("Extend Guest Stay");

            Console.Write("Enter guest ID: ");
            string guestId = Console.ReadLine();

            Guest guest = guests.FirstOrDefault(g => g.GuestId == guestId);
            if (guest == null)
            {
                Console.WriteLine("Error: guest {0} not found.", guestId);
                return;
            }

            if (guest.RoomNumber == "Not Assigned")
            {
                Console.WriteLine("This guest has no active booking to extend.");
                return;
            }

            Console.Write("Enter number of additional nights: ");
            int extraNights;
            if (!int.TryParse(Console.ReadLine(), out extraNights) || extraNights <= 0)
            {
                Console.WriteLine("Error: additional nights must be a positive integer. No change made.");
                return;
            }

            guest.TotalNights += extraNights;   // updated in place

            Console.WriteLine();
            Console.WriteLine("Stay extended!");
            Console.WriteLine("Updated total nights: {0}", guest.TotalNights);
            Console.WriteLine("New total cost:       OMR {0:F2}", guest.CalculateTotalCost(rooms));
        }

        // Case 14 - Highest Revenue Booking
        static void HighestRevenueBooking(List<Room> rooms, List<Guest> guests)
        {
            Console.WriteLine("Highest Revenue Booking ");

            var activeGuests = guests.Where(g => g.RoomNumber != "Not Assigned");

            if (!activeGuests.Any())
            {
                Console.WriteLine("No active bookings recorded.");
                return;
            }

            // Project each active guest into name, room, and total cost,
            // then rank by cost descending and take the single top one.
            var topBooking = activeGuests
                .Select(g => new
                {
                    Name = g.GuestName,
                    Room = g.RoomNumber,
                    TotalCost = g.CalculateTotalCost(rooms)
                })
                .OrderByDescending(x => x.TotalCost)
                .Take(1);

            foreach (var top in topBooking)
            {
                Console.WriteLine("Top earner: {0} - Room {1} - OMR {2:F2}",
                    top.Name, top.Room, top.TotalCost);
            }
        }

        // Case 15 - Guest Pagination Viewer
        static void GuestPaginationViewer(List<Guest> guests)
        {
            Console.WriteLine("Guest Pagination Viewer ");

            if (guests.Count == 0)
            {
                Console.WriteLine("No guests have been registered yet.");
                return;
            }

            int pageSize = 3;

            // Total pages = ceiling of count / pageSize
            int totalPages = (int)Math.Ceiling(guests.Count() / (double)pageSize);

            Console.Write("Enter page number (1-{0}): ", totalPages);
            int page;
            if (!int.TryParse(Console.ReadLine(), out page) || page < 1 || page > totalPages)
            {
                Console.WriteLine("That page does not exist.");
                return;
            }

            // Skip the previous pages, take one page worth of guests
            var pageGuests = guests.Skip((page - 1) * pageSize).Take(pageSize);

            Console.WriteLine();
            Console.WriteLine("Page {0} of {1}:", page, totalPages);
            foreach (Guest g in pageGuests)
            {
                g.DisplayGuest();
            }
        }
    }
}