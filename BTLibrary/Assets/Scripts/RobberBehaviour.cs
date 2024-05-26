using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobberBehaviour : BTAgent
{
    public GameObject backDoor;
    public GameObject frontDoor;
    public GameObject diamond;
    public GameObject[] paintings;
    public GameObject van;

    [Range(0, 1000)]
    public int money = 800;

    public GameObject pickup = null;
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        Sequence steal = new Sequence("Steal Something");

        Leaf hasGotMoney = new Leaf("Has Got Money", HasMoney);
        Leaf goToFrontDoor = new Leaf("Go to Front Door", GoToFrontDoor, 1);
        Leaf goToBackDoor = new Leaf("Go to Back Door", GoToBackDoor, 2);
        Leaf goToDiamond = new Leaf("Go to Diamond", GoToDiamond, 1);
        Leaf goToVan = new Leaf("Go to Van", GoToVan);

        RSelector selectObject = new RSelector("Select Object to Steal");
        for(int i = 0; i < paintings.Length; i++)
        {
            Leaf gta = new Leaf("Go to art" + paintings[i],i, GoToArt);
            selectObject.AddChild(gta);
        }

        Inverter invertMoney = new Inverter("Invert Money");
        invertMoney.AddChild(hasGotMoney);

        DPSelector openDoor = new DPSelector("Open Door");
        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);

        steal.AddChild(invertMoney);
        steal.AddChild(openDoor);
        steal.AddChild(selectObject);
        steal.AddChild(goToVan);

        tree.AddChild(steal);
        tree.PrintTree();
    }

    public Node.Status HasMoney()
    {
        if (money < 500)
            return Node.Status.FAILURE;
        return Node.Status.SUCCESS;
    }

    public Node.Status GoToFrontDoor()
    {
        return GoToDoor(frontDoor);
    }

    public Node.Status GoToBackDoor()
    {
        return GoToDoor(backDoor);
    }

    public Node.Status GoToDoor(GameObject door)
    {
        Node.Status s = GoToLocation(door.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            if (!door.GetComponent<Lock>().isLocked)
            {
                door.GetComponent<NavMeshObstacle>().enabled = false;
                return Node.Status.SUCCESS;
            }
            return Node.Status.FAILURE;
        }
        else
            return s;
    }

    public Node.Status GoToDiamond()
    {
        if (!diamond.activeSelf) return Node.Status.FAILURE;

        Node.Status s = GoToLocation(diamond.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            diamond.transform.parent = this.gameObject.transform;
            pickup = diamond;
        }
        return s;
    }

    public Node.Status GoToArt(int i)
    {
        if(!paintings[i].activeSelf) return Node.Status.FAILURE;
        Node.Status s = GoToLocation(paintings[i].transform.position);
        if (s == Node.Status.SUCCESS)
        {
            paintings[i].transform.parent = this.gameObject.transform;
            pickup = paintings[i];
        }
        return s;
    }

    public Node.Status GoToVan()
    {
        Node.Status s = GoToLocation(van.transform.position);
        if (s == Node.Status.SUCCESS)
        {
            if (!pickup)
                return Node.Status.FAILURE;
            pickup.SetActive(false);
            money += 300;
        }
        return s;
    }
}
