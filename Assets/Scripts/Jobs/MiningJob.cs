using UnityEngine;
using System.Collections;

namespace KofTools{
	public class MiningJob : Job {
		
		Material mat;
		
		bool destroyAtEnd;
		GameObject destroyedAtEnd;
		
		public MiningJob(Vector3 pos):base(pos, JobType.MiningJob){
			int randNum = Random.Range(0, 5);
			if (randNum == 0){
				mat = new EconomicMineral(EconomicMineral.EconomicMineralType.Limonite);
			} else if (randNum == 1){
				mat = new EconomicMineral(EconomicMineral.EconomicMineralType.Cassisterite);
			} else if (randNum == 2){
				mat = new EconomicMineral(EconomicMineral.EconomicMineralType.Malachite);
			} else if (randNum == 3){
				mat = new EconomicMineral(EconomicMineral.EconomicMineralType.Spheralite);
			} else if (randNum == 4){
				mat = new EconomicMineral(EconomicMineral.EconomicMineralType.Bauxite);
			} else {
				mat = new EconomicMineral(EconomicMineral.EconomicMineralType.Limonite);
				Debug.LogError (randNum);
			}
			Debug.Log(mat.materialType);

			percentageComplete = CommandLineControl.miningComplete;
		}
		
		public MiningJob(Vector3 pos, Material nmat):base(pos, JobType.MiningJob){
			mat = nmat;
			percentageComplete = CommandLineControl.miningComplete;
		}
		
		public MiningJob(Vector3 pos, Material nmat, GameObject atEnd):base(pos, JobType.MiningJob){
			mat = nmat;
			percentageComplete = CommandLineControl.miningComplete;
			destroyAtEnd = true;
			destroyedAtEnd = atEnd;
		}
		
		public new bool workOn(double workspeed, Robot robot){
			bool isDone = base.workOn(workspeed, robot);
			double amount = workspeed * (robot.rateOfMining);
			percentageComplete += amount;
			try {
				robot.miningTool.wearPercent += (float)(0.1/robot.miningTool.material.strengthModifier);
				if (robot.miningTool.wearPercent > 100){
					robot.removeMiningTool();
				}
			} catch {
				
			}
			
			SoundController.playEffect("Jackhammer", false, Position);
			
			if (isDone){
				GameObject obj = (GameObject) GameObject.Instantiate(UnityEngine.Resources.Load("Prefabs/OrePrefab"));
				obj.transform.position = Position;
				Ore r = (Ore)obj.GetComponent("Ore");

				//Debug.Log (mat.materialType.ToString());
				r.starterate(mat);

				SoundController.playEffect("EjectRock", false, Position);
				
				GameObject.Destroy(destroyedAtEnd);
			}
			
			return isDone;
			
		}
		
		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
		
		}
	}
}
