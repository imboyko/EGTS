namespace Egts.Processing
{
    interface IProcessible
    {
        IEgtsProcessor Processor { get; }

        void SetProcessor(IEgtsProcessor processor);
        void Process(ref ProcessingResult result);
    }
}
