namespace WindowsServices.PrintMergingPages.Client
{
    public interface IMergePagesClient
    {
        bool Start();
        bool Stop();
    }
}
