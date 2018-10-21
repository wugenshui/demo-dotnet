namespace DesignPattern
{
    class Woman : Human
    {
        public override void Accept(Action visitor)
        {
            visitor.GetWomanConclusion(this);
        }
    }
}