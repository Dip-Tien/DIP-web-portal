using Microsoft.AspNetCore.Components;

namespace MyWebERP.Model
{
    public class Apartment
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Floor { get; set; }
        public double Area { get; set; }   // Diện tích m²
        public double X { get; set; }
        public double Y { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Status { get; set; } = "Available"; // Sold, Reserved...

        // URL hình ảnh, optional
        public string? ImageUrl { get; set; }

        // ElementReference để JSInterop
        public ElementReference RectRef { get; set; }
        // Màu theo Status
        public string Color => Status switch
        {
            "Available" => "lightgreen",
            "Sold" => "lightcoral",
            "Reserved" => "gold",
            _ => "lightgray"
        };
    }

}
