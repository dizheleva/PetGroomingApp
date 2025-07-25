﻿namespace PetGroomingApp.Data.Seeding.Dtos
{
    public class GroomerInputModel
    {
        public required string Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Description { get; set; }
    }
}
