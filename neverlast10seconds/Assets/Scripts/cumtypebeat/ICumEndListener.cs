namespace cumtypebeat
{
    public interface ICumEndListener
    {
        void End();
        bool Ended { get; set; }
    }
}