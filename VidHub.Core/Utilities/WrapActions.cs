namespace VidHub.Core.Utilities
{
    public class WrapActions<T>(Action<T> preAction, Action<T> postAction)
    {
        public static Action<T> NoAction => _ => { };


        public Action<T> PreAction { get; } = preAction;
        public Action<T> PostAction { get; } = postAction;


        public WrapActions(Action<T> action) : this(action, action) { }

        public static implicit operator WrapActions<T>(Action<T> action)
        {
            return new(action);
        }
    }
}
