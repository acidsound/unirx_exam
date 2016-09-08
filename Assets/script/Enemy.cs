using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;

public class Enemy : MonoBehaviour {
	CompositeDisposable disposables = new CompositeDisposable();
	// Use this for initialization
	void Start () {
		var hosts = GameObject.FindGameObjectsWithTag ("host");
		this.OnMouseDownAsObservable ()
			.Select (_ => 1)
			.Scan ((acc, current) => acc + current)
			.Subscribe (o => {
			foreach (GameObject host in hosts) {
					host.GetComponent<Host>().VectorYSubject.OnNext(1);
			}
		}).AddTo (disposables);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnDestroy () {
		disposables.Dispose ();
	}
}
