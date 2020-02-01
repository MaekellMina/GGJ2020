using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSettings
{
	public List<Vector2> sequence;  // (x,y) where x is index of action-type (0 = Hammer, 1 = Furnace) and y is level reference of that action
	public int brokenness;  // 0 = shattered, 1 = chipped, 2 = bent
	public int weaponType; //0 = sword

	public LevelSettings(string sequenceText, int brokenness, int weaponType)
	{
		sequence = new List<Vector2>();
		for (int i = 0; i < sequenceText.Length; i++)
		{
			int x = -1;
			if (sequenceText[i] == 'H') //H for Hammer
				x = 0;
			else if (sequenceText[i] == 'F')    //F for Furncace
				x = 1;
         
			string seqIndex = sequenceText[i + 1].ToString();
			i++;
            //for 2 digit numbers
			if(i+1 < sequenceText.Length)
			{
				int num = 0;
				if(int.TryParse(sequenceText[i + 1].ToString(), out num))
				{               
					seqIndex += sequenceText[i + 1].ToString();               
                    i++;

				}
					
			}

			int y = int.Parse(seqIndex) - 1;    //parse to zero-base (1 = 0, 2 = 1) for array index referencing

			sequence.Add(new Vector2(x, y));
		}

		this.brokenness = brokenness;
		this.weaponType = weaponType;
	}
}
