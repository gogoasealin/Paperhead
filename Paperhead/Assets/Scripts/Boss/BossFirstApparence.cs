using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFirstApparence : MonoBehaviour {

    Vector2 destiantion = Vector2.zero;
    BossHp bossHp;

    void Start()
    {
        bossHp = GetComponent<BossHp>();
    }

	void Update () {
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), destiantion, 3 * Time.deltaTime);
        if(transform.position.x == destiantion.x && transform.position.y == destiantion.y)
        {
            bossHp.enabled = true;
            Destroy(GetComponent<BossFirstApparence>());
        }
	}

}
