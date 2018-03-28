using System;

namespace DragonCave
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			String command;
			Model game = Model.GetModel ();
			Output output = new Output ();

			output.StartDisplay ();
			game.Detect ();
			output.Update ();

			while (!game.GameIsOver()) {
				command = Console.ReadLine ().ToLower();

				switch (command) {
				case "f":
					game.GoForward ();
					output.GoForward ();
					break;
				case "r": 
					//fall through
				case "l":
					game.ChangeDirection (command);
					break;
				case "g":
					output.GetGold ();
					game.GetGold ();
					break;
				case "s":
					game.ShootArrow ();
					output.ShootArrow ();
					game.RemoveArrow ();
					break;
				case "c":
					game.ClimbOut ();
					output.ClimbOut();
					break;
				case "q":
					game.Quit ();
					output.Quit ();
					break;
				case "x":
					output.Cheat ();
					break;
				}

				game.Detect ();
				output.Update ();
			}
		}
	}
}