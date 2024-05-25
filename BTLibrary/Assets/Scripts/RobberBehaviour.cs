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
    public int selectedPainting = 0;
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
        Leaf goToPainting = new Leaf("Go to Painting", GoToPainting, 2);
        Leaf goToVan = new Leaf("Go to Van", GoToVan);

        Inverter invertMoney = new Inverter("Invert Money");
        invertMoney.AddChild(hasGotMoney);

        DPSelector openDoor = new DPSelector("Open Door");
        openDoor.AddChild(goToFrontDoor);
        openDoor.AddChild(goToBackDoor);

        PSelector stealSomething = new PSelector("Steal Something");
        stealSomething.AddChild(goToDiamond);
        stealSomething.AddChild(goToPainting);

        steal.AddChild(invertMoney);
        steal.AddChild(openDoor);
        steal.AddChild(stealSomething);
        steal.AddChild(goToVan);

        tree.AddChild(steal);
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

    public Node.Status GoToPainting()
    {
        Node.Status s = Node.Status.FAILURE;
        if (paintings[selectedPainting].activeSelf)
        {
            s = GoToLocation(paintings[selectedPainting].transform.position);
            if (s == Node.Status.SUCCESS)
            {
                paintings[selectedPainting].transform.parent = this.gameObject.transform;
                pickup = paintings[selectedPainting];
            }
        }
        else
        {
            selectedPainting++;
            if (selectedPainting >= paintings.Length)
            {
                selectedPainting = 0;
                return Node.Status.FAILURE;
            }
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
