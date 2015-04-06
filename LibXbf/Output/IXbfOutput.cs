namespace LibXbf.Output
{
    public interface IXbfOutput<T>
    {
        T GetOutput(XbfFile file);
    }
}
