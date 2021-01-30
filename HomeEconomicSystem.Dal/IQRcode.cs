namespace HomeEconomicSystem.Dal
{
    public interface IQRcode
    {
        string FileName { get; }
        byte[] ImageStream { get; }
    }
}