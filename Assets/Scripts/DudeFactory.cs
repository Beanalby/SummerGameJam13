using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DudeFactory : MonoBehaviour {
    public Dude dudePrefab;
    public Texture2D humanScience, humanReligion, humanFreedom, humanLaw;
    public Texture2D robotScience, robotReligion, robotFreedom, robotLaw;

    public List<Dude> MakeDudes(int xMin, int xMax, int y, int num,
            bool isRobot, bool isReligious, bool isLaw) {
        List<Dude> dudes = new List<Dude>();

        // they'll all be either robot or human, make the rest a mix of the two
        int i;
        for (i = 0; i < num; i++) {
            Dude dude = ((GameObject)GameObject.Instantiate(dudePrefab.gameObject)).GetComponent<Dude>();
            dude.name = "dude" + i;
            dude.xMin = xMin;
            dude.xMax = xMax;
            dude.x = Random.Range(xMin, xMax);
            dude.y = y;
            if (i < num / 2) {
                if (isRobot) {
                    if (isReligious) {
                        dude.primary = robotReligion;
                    } else {
                        dude.primary = robotScience;
                    }
                } else {
                    if (isReligious) {
                        dude.primary = humanReligion;
                    } else {
                        dude.primary = humanScience;
                    }
                }
            } else {
                if (isRobot) {
                    if (isLaw) {
                        dude.primary = robotLaw;
                    } else {
                        dude.primary = robotFreedom;
                    }
                } else {
                    if (isLaw) {
                        dude.primary = humanLaw;
                    } else {
                        dude.primary = humanFreedom;
                    }
                }
            }
        }
        return dudes;
    }
}
