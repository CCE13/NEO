using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckStaminaNode : CompositeNode
{
    public bool test;
    public float amountItCanDash;
    private float minusAmount;
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        //Checks if the there are more then two nodes connected to it
        if (children.Count > 2)
        {
            return State.Failure;
        }

        if (children.Count == 1)
        {
            //Runs the children if there is only one child connect
            return children[0].Update();
        }

       
        

        if(enemyInfo.enemyMovement.goToWall == true && enemyInfo.enemyMovement.dashing == false)
        {
        
            enemyInfo.staminaImage.fillAmount = amountItCanDash;
        }

        if (amountItCanDash - enemyInfo.staminaImage.fillAmount < amountItCanDash )
        {

            if (enemyInfo.enemyMovement.dashing == false)
            {
                return children[0].Update();
            }
            else
            {
                //gets the percentage to decrease as the fill amount must be always 1
                minusAmount = 1 / amountItCanDash;
                enemyInfo.staminaImage.fillAmount -= minusAmount;
                return children[0].Update();
            }

            

        }
        else
        {
            
            return children[1].Update();
        }

        



    }
}
