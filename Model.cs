using System;

namespace DragonCave
{
	public class Model
	{
		private String[,] grid;
		private int posRow, posCol; //Player location
		private int dragonRow, dragonCol; //Dragon location (used in ShootArrow)
		private String direction;
		private Boolean gameOver, hasArrow, hasGold, dragonDead;
		private Boolean inPit, nearPit, inDragon, nearDragon, inGold, atEntrance, canMove;

		private static Model game;

		public Model ()
		{
			grid = new String[4, 4] { 
				{ "P", ".", ".", "." }, 
				{ "P", ".", ".", "." }, 
				{ ".", "G", ".", "." },
				{ ".", "E", "D", "P" }	
			};

			grid = RandomizeMap (grid);

			//Store player position and dragon position
			for (int i = 0; i < 4; i++) {
				for (int j = 0; j < 4; j++) {
					String rm = grid [i, j];
					if (rm == "D") {
						dragonRow = i;
						dragonCol = j;
					} else if (rm == "E") {
						posRow = i;
						posCol = j;
					}
				}
			}

			direction = "EAST";
			gameOver = false;
			hasArrow = true;
			hasGold = false;
			dragonDead = false;
		}

		/**RandomizeMap
		 * Randomizes contents of 4x4 array.
		 */
		public String[,] RandomizeMap(String[,] grid)
		{
			Random rnd = new Random ();
			int rndA, rndB;
			String temp;
			for (int i = 0; i < 4; i++) {
				for (int j = 0; j < 4; j++) {
					rndA = rnd.Next (4);
					rndB = rnd.Next (4);
					temp = grid [i, j];
					grid [i, j] = grid [rndA, rndB];
					grid [rndA, rndB] = temp;
				}
			}

			return grid;
		}

		/**Detect
		 * Checks current room and surrounding rooms for pits, dragon, and gold.
		 */
		public void Detect()
		{
			String currentRm = grid [posRow, posCol];

			//Check current room
			atEntrance = false;
			inGold = false;
			if (currentRm == "E") {
				atEntrance = true;
			} else if (currentRm == "G") {
				inGold = true;
			} else if (currentRm == "P") {
				inPit = true;
				this.gameOver = true;
			} else if (currentRm == "D") {
				inDragon = true;
				this.gameOver = true;
			}

			//Check surrounding rooms
			nearPit = false;
			nearDragon = false;
			if (posRow < 3) {
				if (grid [posRow + 1, posCol] == "P") {
					nearPit = true;
				} else if (grid [posRow + 1, posCol] == "D") {
					nearDragon = true;
				}
			}
			if (posRow > 0) {
				if (grid [posRow - 1, posCol] == "P") {
					nearPit = true;
				} else if (grid [posRow - 1, posCol] == "D") {
					nearDragon = true;
				} 
			}
			if (posCol < 3) {
				if (grid [posRow, posCol + 1] == "P") {
					nearPit = true;
				} else if (grid [posRow, posCol + 1] == "D") {
					nearDragon = true;
				}
			}
			if (posCol > 0) {
				if (grid [posRow, posCol - 1] == "P") {
					nearPit = true;
				} else if (grid [posRow, posCol - 1] == "D") {
					nearDragon = true;
				}
			}
		}

		/**GoForward
		 * Moves character to room straight ahead.
		 * (Doesn't move if in front of a wall.)
		 */
		public void GoForward()
		{
			switch (direction) {
			case "NORTH":
				if (posRow > 0) {
					posRow -= 1;
					canMove = true;
				} else {
					canMove = false;
				}
				break;
			case "SOUTH":
				if (posRow < 3) {
					posRow += 1;
					canMove = true;
				} else {
					canMove = false;
				}
				break;
			case "WEST":
				if (posCol > 0) {
					posCol -= 1;
					canMove = true;
				} else {
					canMove = false;
				}
				break;
			case "EAST":
				if (posCol < 3) {
					posCol += 1;
					canMove = true;
				} else {
					canMove = false;
				}
				break;

			default:
				canMove = false;
				break;
			}
		}

		/**ChangeDirection
		 * Changes direction based on player turning L or R.
		 */
		public void ChangeDirection(String cmd)
		{
			//Change direction based on dir turned
			String[] directions = { "NORTH", "EAST", "SOUTH", "WEST" };
			int currentIndex = Array.IndexOf (directions, direction);
			if (cmd == "r") {
				currentIndex += 1;
			} else { //cmd == "l"
				currentIndex -= 1;
			}


			//Handle first and last indexes
			if (currentIndex < 0) {
				currentIndex = 3;
			} else if (currentIndex > 3) {
				currentIndex = 0;
			}

			direction = directions [currentIndex];
		}

		/**GetGold
		 * Player picks up gold (if in current room).
		 */
		public void GetGold()
		{
			if (inGold) {
				grid [posRow, posCol] = ".";
				hasGold = true;
			}
		}

		/**ShootArrow
		 * Shoots arrow in player's current facing direction.
		 * Kills the dragon if in straight path from player.
		 */
		public void ShootArrow()
		{
			if (hasArrow) {
				switch (direction) {
				case "EAST":
					if (posRow == dragonRow && dragonCol > posCol) {
						dragonDead = true;
					}
					break;
				case "WEST":
					if (posRow == dragonRow && dragonCol < posCol) {
						dragonDead = true;
					}
					break;
				case "SOUTH":
					if (posCol == dragonCol && dragonRow > posRow) {
						dragonDead = true;
					}
					break;
				case "NORTH":
					if (posCol == dragonCol && dragonRow < posRow) {
						dragonDead = true;
					}
					break;
				}

				if (dragonDead) {
					grid [dragonRow, dragonCol] = ".";
				}
			}
		}

		/**RemoveArrow
		 * Player will no longer have an arrow.
		 */
		public void RemoveArrow()
		{
			this.hasArrow = false;
		}

		/**ClimbOut
		 * Climb out of the cave (if by the entrance) whether goals are accomplished or not.
		 * Game ends.
		 */
		public void ClimbOut()
		{
			if (atEntrance) {
				gameOver = true;
			}
		}

		/**Quit
		 * End game.
		 */
		public void Quit()
		{
			gameOver = true;
		}

		public static Model GetModel()
		{
			if (game == null) {
				game = new Model();
			}
			return game;
		}

		public String[,] GetGrid()
		{
			return this.grid;
		}

		public String GetDirection()
		{
			return this.direction;
		}

		public int GetPosRow()
		{
			return this.posRow;
		}

		public int GetPosCol()
		{
			return this.posCol;
		}

		public Boolean GameIsOver()
		{
			return this.gameOver;
		}

		public Boolean HasArrow()
		{
			return this.hasArrow;
		}

		public Boolean HasGold()
		{
			return this.hasGold;
		}

		public Boolean DragonDead()
		{
			return this.dragonDead;
		}
		
		public Boolean InPit()
		{
			return this.inPit;
		}

		public Boolean NearPit()
		{
			return this.nearPit;
		}

		public Boolean InDragon()
		{
			return this.inDragon;
		}

		public Boolean NearDragon()
		{
			return this.nearDragon;
		}

		public Boolean InGold()
		{
			return this.inGold;
		}

		public Boolean AtEntrance()
		{
			return this.atEntrance;
		}

		public Boolean CanMove()
		{
			return this.canMove;
		}
	}
}