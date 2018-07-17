using System;

namespace FSM {
	public class EventFSM<T> {
		State<T> current;

		public EventFSM(State<T> initial) {
			current = initial;
			current.Enter(default(T));
		}

		public void Feed(T input) {
			State<T> newState;

			if (current.Feed(input, out newState)) {
				current.Exit(input);
				current = newState;
				current.Enter(input);
			}
		}

		public State<T> Current { get { return current; } }

		public void Update() {
			current.Update();
		}
	}
}