using UnityEngine;

public class Point{
	public int x;
	public int y;

	public Point(int x, int y){
		this.x=x;
		this.y=y;
	}

	public Point() : this(0,0){

	}

	public Point(int size) : this(size,size){
		
	}

	public Vector2 ToVector2(){
		return new Vector2(x,y);
	}

    public override string ToString()
    {
        return string.Format("({0}, {1})", x, y);
    }

} 