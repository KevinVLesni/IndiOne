using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public GameObject shotgunBullet;
    public GameObject revolverBullet;
    private Vector2 CenterOfScreen { get { return new Vector2(Screen.width/2, Screen.height/2); } }
    private Vector2 CursorRelativeOfHero { get { return new Vector2 (Input.mousePosition.x, Input.mousePosition.y)-CenterOfScreen; } }
    public Vector2 mousePos;

    private Rigidbody2D rb;

    private Animator anim;

    public int dashLevel;
    public int dashCount = 0;
    public int weaponID;
    public int shotgunPatronsInBarrel;
    public int shotgunPatrons;
    public int revolverPatronsInBarrel;
    public int revolverPatrons;

    public bool canDash;
    public bool canSecondDash;
    private bool isDashing;
    public bool canTakeDamage;

    public float dashDistanceOne; // базовая дистанция рывка
    public float dashDistanceTwo; // прокачанная дистанция рывка

    public float dashingTimeLvlOne; // базовые фреймы неуязвимости
    public float dashingTimeLvlTwo; // прокачанные фреймы неуязвимости

    public float dashCooldownLvlOne; // базовое кд
    public float dashCooldownLvlTwo; // прокачанное кд
    public float dashCooldownLvlThree; // прокачанное кд 2

    public float normalSpeed;
    public float runSpeed;
    public float currentSpeed;
    public float lowerSpeed;

    public Transform shotgunPos;
    public Transform revolverPos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        HeroRotate();
        UpdateControllerValue();
        if (Input.GetKey(KeyCode.Space) && canDash)
        {
            if (dashLevel == 1)
                StartCoroutine(DashLvlOne());
            else if (dashLevel == 2)
                StartCoroutine(DashLvlTwo());
            else if (dashLevel == 3)
                StartCoroutine(DashLvlThree());
            else if (dashLevel == 4)
                StartCoroutine(DashLvlFour());
            else if (dashLevel == 5)
                StartCoroutine(DashLvlFive());
        }

        if(dashCount == 1 && dashLevel == 5 && Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(SecondDash());
        }
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        Step();
        if (Input.GetMouseButtonDown(0))
        {
            if (weaponID == 1)
            {
                anim.SetTrigger("ShotgunShot");
                anim.SetBool("Shooting", true);
            }
            else if (weaponID == 2)
            {
                anim.SetTrigger("RevolverShot");
                anim.SetBool("Shooting", true);
            }
        }
        
        if(Input.GetKeyDown(KeyCode.R) && shotgunPatronsInBarrel != 2)
        {
            anim.SetTrigger("ShotgunReload");
            anim.SetBool("Shooting", true);
        }     
        if (anim.GetBool("Shooting"))
        {
            currentSpeed = lowerSpeed;
        }
        else if (!anim.GetBool("Shooting") && !anim.GetBool("Run"))
        {
            currentSpeed = normalSpeed;
        }
        else if (!anim.GetBool("Shooting") && anim.GetBool("Run"))
        {
            currentSpeed = runSpeed;
        }

        if(Input.GetKeyDown("1"))
        {
            weaponID = 1;
        }
        else if (Input.GetKeyDown("2"))
        {
            weaponID = 2;
        }

        if (isDashing)
            canTakeDamage = false;
        else
            canTakeDamage = true;

    }

    //------------------------------СТРЕЛЬБА-----------------------------------------------------------------
    public void ShootingOn()
    {
        anim.SetBool("Shooting", true);
    }
    public void ShootingOff()
    {
        anim.SetBool("Shooting", false);
    }
    public void ShotgunShot()
    {
        if(shotgunPatronsInBarrel > 0)
        {
            shotgunPatronsInBarrel -= 1;
            Instantiate(shotgunBullet, shotgunPos.position, Quaternion.Euler(0, 0, Mathf.Atan2(CursorRelativeOfHero.y, CursorRelativeOfHero.x) * 180 / Mathf.PI + 90));
            Instantiate(shotgunBullet, shotgunPos.position, Quaternion.Euler(0, 0, Mathf.Atan2(CursorRelativeOfHero.y, CursorRelativeOfHero.x) * 180 / Mathf.PI + 90 + 13));
            Instantiate(shotgunBullet, shotgunPos.position, Quaternion.Euler(0, 0, Mathf.Atan2(CursorRelativeOfHero.y, CursorRelativeOfHero.x) * 180 / Mathf.PI + 90 + 26));
            Instantiate(shotgunBullet, shotgunPos.position, Quaternion.Euler(0, 0, Mathf.Atan2(CursorRelativeOfHero.y, CursorRelativeOfHero.x) * 180 / Mathf.PI + 90 - 13));
            Instantiate(shotgunBullet, shotgunPos.position, Quaternion.Euler(0, 0, Mathf.Atan2(CursorRelativeOfHero.y, CursorRelativeOfHero.x) * 180 / Mathf.PI + 90 - 26));
        }
    }
    public void ShotgunReload()
    {
        if (shotgunPatronsInBarrel < 2 && shotgunPatrons > 0)
        {
            shotgunPatronsInBarrel += 1;
            shotgunPatrons -= 1;
        }
    }

    public void RevolverShot()
    {
        if (revolverPatronsInBarrel > 0)
        {
            revolverPatronsInBarrel -= 1;
            Instantiate(revolverBullet, revolverPos.position, Quaternion.Euler(0, 0, Mathf.Atan2(CursorRelativeOfHero.y, CursorRelativeOfHero.x) * 180 / Mathf.PI + 90));
        }
    }
    public void RevolverReload()
    {
        if (revolverPatronsInBarrel < 6 && revolverPatrons > 0)
        {
            revolverPatronsInBarrel += 1;
            revolverPatrons -= 1;
        }
    }

    //------------------------------ПЕРЕДВИЖЕНИЕ-----------------------------------------------------------------
    public void Step()
    {
        rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized * currentSpeed;
    }
    public void HeroRotate()
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(CursorRelativeOfHero.y, CursorRelativeOfHero.x) *180/Mathf.PI+90);
    }
    public void UpdateControllerValue()
    {
        anim.SetFloat("Vertical", Input.GetAxisRaw("Vertical"));
        anim.SetFloat("Horizontal", Input.GetAxisRaw("Horizontal"));
        anim.SetBool("Move", (Input.GetAxisRaw("Vertical")!=0 || Input.GetAxisRaw("Horizontal")!=0) ? true : false); ;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            anim.SetBool("Run", true);
            currentSpeed = runSpeed;
        }
        else
        {
            anim.SetBool("Run", false);
            currentSpeed = normalSpeed;
        }
    }
    private IEnumerator DashLvlOne() // длина 1 лвл, фреймы 1 лвл, кд 1 лвл
    {
        canDash = false;
        isDashing = true;
        rb.AddForce(mousePos.normalized * dashDistanceOne, ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashingTimeLvlOne);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldownLvlOne);
        canDash = true;
    }
    private IEnumerator DashLvlTwo() // длина 1 лвл, фреймы 1 лвл, кд 2 лвл
    {
        canDash = false;
        isDashing = true;
        rb.AddForce(mousePos.normalized * dashDistanceTwo, ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashingTimeLvlOne);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldownLvlTwo);
        canDash = true;
    }
    private IEnumerator DashLvlThree() // длина 2 лвл, фреймы 2 лвл, кд 2 лвл
    {
        canDash = false;
        isDashing = true;
        rb.AddForce(mousePos.normalized * dashDistanceTwo, ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashingTimeLvlTwo);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldownLvlTwo);
        canDash = true;
    }
    private IEnumerator DashLvlFour() // длина 2 лвл, фреймы 2 лвл, кд 3 лвл
    {
        canDash = false;
        isDashing = true;
        rb.AddForce(mousePos.normalized * dashDistanceTwo, ForceMode2D.Impulse);
        yield return new WaitForSeconds(dashingTimeLvlTwo);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldownLvlThree);
        canDash = true;
    }
    private IEnumerator DashLvlFive() // длина 2 лвл, фреймы 2 лвл, кд 3 лвл, двойной дэш
    {
        canDash = false;
        isDashing = true;
        rb.AddForce(mousePos.normalized * dashDistanceTwo, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.1f);
        dashCount = 1;
        yield return new WaitForSeconds(dashingTimeLvlTwo);
        isDashing = false; 
        yield return new WaitForSeconds(dashCooldownLvlThree);
        canDash = true;
    }
    private IEnumerator SecondDash()
    {
        canSecondDash = false;
        rb.AddForce(mousePos.normalized * dashDistanceTwo, ForceMode2D.Impulse);
        dashCount = 0;
        yield return new WaitForSeconds(0.7f);
        canSecondDash = false; 
        yield return new WaitForSeconds(dashCooldownLvlThree - 1f);
        canSecondDash = true;
    }
}
