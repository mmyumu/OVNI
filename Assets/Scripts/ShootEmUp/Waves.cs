using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waves : MonoBehaviour {
    private List<Wave> waves;

    public Waves() {
        waves = new List<Wave>();

        waves.Add(basicWave1());
    }

    private static Wave basicWave1() {
        return new Wave();
    }
}

public class Wave {
    public Wave(params Enemy[] enemies) {

    }
}

public class Enemy {

}