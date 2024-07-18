namespace MyWebAPI.Models
{
    public class EmpleadoSettings : IEmpleadoSettings
    {
        public string Server { get; set; }
        public string Database { get; set; }
        public string Collections { get; set; }
    }

    public interface IEmpleadoSettings
    {
        string Server { get; set; }
        string Database { get; set; }
        string Collections { get; set; }
    }
}
