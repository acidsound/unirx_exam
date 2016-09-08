using UnityEngine;
using System.Collections;
using UniRx;
using UniRx.Triggers;
using System;

public class Host : MonoBehaviour {
	CompositeDisposable disposables = new CompositeDisposable();

	public Subject<int> VectorYSubject;

	public IObservable<int> VectorYAsObservable() {
		// Lazy Subject
		return (VectorYSubject ?? (VectorYSubject = new Subject<int>())).AsObservable();
	}

	// Use this for initialization
	void Start () {
		/* single click test */
		var mouseDownStream = this.OnMouseDownAsObservable ();
		mouseDownStream
			.Subscribe (_ => 
			gameObject.GetComponent<Renderer> ().material.color = Color.cyan
		).AddTo (disposables);

		/* double click test */
		mouseDownStream
			.Buffer(mouseDownStream.Throttle(TimeSpan.FromMilliseconds(250)))
			.Where(x=>x.Count>1)
			.Subscribe (_ => 
			gameObject.GetComponent<Renderer> ().material.color = Color.red
		).AddTo (disposables);

		Observable.Interval (TimeSpan.FromMilliseconds (1000)).Subscribe (_ =>
			VectorYSubject.OnNext(-1)
		).AddTo (disposables);

		VectorYAsObservable().Subscribe (y => transform.Translate(new Vector3(0, y, 0)));
	}
	// Update is called once per frame
	void Update () {
	
	}
	void OnDestroy () {
		disposables.Dispose ();
	}
}
