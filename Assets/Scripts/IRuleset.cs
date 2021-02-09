namespace Lumpn.Storylets
{
    public interface IRuleset
    {
        Rule Query(IState state);
    }
}
