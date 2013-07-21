using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DudeFactory : MonoBehaviour {
    public Dude dudePrefab;
    public Texture2D humanScience, humanReligion, humanFreedom, humanLaw;
    public Texture2D[] humanScienceAnim, humanReligionAnim, humanFreedomAnim, humanLawAnim;

    public Texture2D robotScience, robotReligion, robotFreedom, robotLaw;
    public Texture2D[] robotScienceAnim, robotReligionAnim, robotFreedomAnim, robotLawAnim;

    public List<Dude> MakeDudes(int xMin, int xMax, int y, int num,
            bool isRobot, bool isReligious, bool isLaw) {
        List<Dude> dudes = new List<Dude>();

        Debug.Log("Creating dudes with law=" + isLaw);
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
                        dude.texStatic = robotReligion;
                        dude.texAnim = robotReligionAnim;
                    } else {
                        dude.texStatic = robotScience;
                        dude.texAnim = robotScienceAnim;
                    }
                } else {
                    if (isReligious) {
                        dude.texStatic = humanReligion;
                        dude.texAnim = humanReligionAnim;
                    } else {
                        dude.texStatic = humanScience;
                        dude.texAnim = humanScienceAnim;
                    }
                }
            } else {
                if (isRobot) {
                    if (isLaw) {
                        dude.texStatic = robotLaw;
                        dude.texAnim = robotLawAnim;

                    } else {
                        dude.texStatic = robotFreedom;
                        dude.texAnim = robotFreedomAnim;
                    }
                } else {
                    if (isLaw) {
                        dude.texStatic = humanLaw;
                        dude.texAnim = humanLawAnim;
                    } else {
                        dude.texStatic = humanFreedom;
                        dude.texAnim = humanFreedomAnim;
                    }
                }
            }
        }
        return dudes;
    }
}
