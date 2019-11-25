using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ShootBullets : NetworkBehaviour
{

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private float bulletSpeed;

    void Update()
    {
        if (this.isLocalPlayer && Input.GetKeyDown(KeyCode.Space))
        {
            this.CmdShoot(transform.position);
        }
    }

    // Commands - Only run on Server
    // Rpc (Remote Procedure Call) - Runs on Client but called from Server

    [ClientRpc]
    void RpcClientShot(Vector3 position)
    {
        GameObject bullet = Instantiate(bulletPrefab, position, Quaternion.identity);
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.up * bulletSpeed;
        Destroy(bullet, 1.0f);
    }

    [Command]
    void CmdShoot(Vector3 position)
    {
        // Tell the client to spawn a bullet
        RpcClientShot(position);
    }
}