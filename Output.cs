using System;

namespace DragonCave
{
	public class Output
	{
		private Model game = Model.GetModel();

		/**Update
		 * Prints current environment effect.
		 */
		public void Update() {
			if (game.InPit ()) {
				Console.WriteLine ("You are fall into the pit!");
				Console.WriteLine ("You are dead !!!");
			} else if (game.InDragon ()) {
				Console.WriteLine ("You and the dragon lock eyes. You share a moment.\n" +
					"He devours you whole.");
				Console.WriteLine ("You are dead !!!");
			} else if (!game.GameIsOver ()) {
				if (game.InGold ()) {
					Console.WriteLine ("The room is glittering!");
				}
				if (game.NearDragon ()) {
					Console.WriteLine ("A foul stench is in the air!");
				}
				if (game.NearPit ()) {
					Console.WriteLine ("A damp breeze is in the air!");
				}
			}

			if (!game.GameIsOver ()) {
				Console.WriteLine ("\nYou are facing " + game.GetDirection ());
				Console.Write ("Enter Command: ");
			}
		}

		/**GoForward
		 * Display movement.
		 */
		public void GoForward()
		{
			if (game.CanMove()) {
				Console.WriteLine("You walk into the next room.");
			} else {
				Console.WriteLine ("You bump into a wall.");
			}
		}

		/**GetGold
		 * Display attempt to pick up gold.
		 */
		public void GetGold()
		{
			if (game.InGold()) {
				Console.WriteLine ("You got the gold !!!");
			} else {
				Console.WriteLine ("Nothing happens.");
			}
		}

		/**ShootArrow
		 * Display shoot attempt (shoot success/failure, kill success/failure).
		 */
		public void ShootArrow()
		{
			if (game.HasArrow()) {

				if (game.DragonDead()) {
					Console.WriteLine ("You hear a giant roar in the distance!!!");
				} else {
					Console.WriteLine ("You hear a thud in the distance.");
				}
			} else {
				Console.WriteLine ("You don't have an arrow.");
			}
		}

		/**ClimbOut
		 * Displays escape. If they found the gold, slayed the dragon, won.
		 */
		public void ClimbOut()
		{
			if (game.AtEntrance()) {
				Console.WriteLine ("You escape the cave!!!");

				if (!game.HasGold()) {
					Console.WriteLine ("But there is still gold to be found.");
				} else if (!game.DragonDead()) {
					Console.WriteLine ("But there is still a dragon to slay.");
				} else {
					Console.WriteLine ("!!!!!! You Win !!!!!!");
				}
			} else {
				Console.WriteLine ("Nothing happens.");
			}
		}

		/**Quit
		 * Verbally abuse the player for quitting.
		 */
		public void Quit()
		{
			Console.WriteLine ("Quitter!!!");
		}

		/**Cheat
		 * Displays map and player location.
		 */
		public void Cheat()
		{
			for (int i = 0; i < 4; i++) {
				String row = "";
				for (int j = 0; j < 3; j++) {
					row += game.GetGrid () [i, j] + "\t";
				}

				row += game.GetGrid () [i, 3];
				Console.WriteLine (row);
			}

			Console.WriteLine ("Position (x, y): (" + (game.GetPosCol() + 1) + ", " + (game.GetPosRow() + 1) + ")");
		}

		/**StartDisplay
		 * Displays welcome message and instructions.
		 */
		public void StartDisplay()
		{
			Console.WriteLine ("Welcome to Dragon Cave!\n" +
				"Try to find the gold and return here to climb back out.\n" +
				"You have 1 arrow that you can shoot.\n" +
				"Try the following commands:\n" +
				"Move (F)orward, Turn (L)eft, Turn (R)ight,\n" +
				"(G)rab the Gold, (S)hoot the Arrow, (C)limb out.\n" +
				"(Q)uit the game, Use (X) to cheat.\n");
		}
	}
}