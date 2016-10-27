using UnityEngine;
using System.Collections.Generic;

public class MagneticBehaviour : MonoBehaviour {
    public Collider2D magneticCollider;
    public Side[] sides;
    public MagneticType magneticForce;
    private List<LinePainter> linePainters;
    private float lineWidthMax = 0.06f;
    private float lineWidthMin = 0.02f;

    private const float SPEED = 1f;

    public enum Side{ TOP, BOTTOM, RIGHT, LEFT}
    public enum MagneticType { ATTRACT, REPEL}


    void Start() {
        linePainters = createLinePainters();
    }

    void Update() {
        foreach(LinePainter linePainter in linePainters) {
            linePainter.updateLines();
        }
    }
    
    private List<LinePainter> createLinePainters() {
        List<LinePainter> linePainters = new List<LinePainter>();
        float radious = ((CircleCollider2D)magneticCollider).radius;
        // num_lines Not 100% accurate, radious is not the magnetic distance (it's taking the center, 
        // but should start on the side of the floor. distance = radious - spriteHeight / 2)
        int num_lines = (int)Mathf.Max(radious * 4f, 4f);
        float spriteWidth = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        float spriteHeight = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
        foreach (Side side in sides) {
            if(side == Side.BOTTOM) {
                linePainters.Add(new LinePainterBottom(gameObject.transform.position, spriteWidth,
                                                        spriteHeight, radious, 
                                                        lineWidthMin, lineWidthMax, num_lines, SPEED));
            } 
            else if (side == Side.TOP) {
                linePainters.Add(new LinePainterTop(gameObject.transform.position, spriteWidth,
                                                        spriteHeight, radious,
                                                        lineWidthMin, lineWidthMax, num_lines, SPEED));
            }
            else if (side == Side.RIGHT) {
                linePainters.Add(new LinePainterRight(gameObject.transform.position, spriteWidth,
                                                        spriteHeight, radious,
                                                        lineWidthMin, lineWidthMax, num_lines, SPEED));
            }
            else if (side == Side.LEFT) {
                linePainters.Add(new LinePainterLeft(gameObject.transform.position, spriteWidth,
                                                        spriteHeight, radious,
                                                        lineWidthMin, lineWidthMax, num_lines, SPEED));
            }
        }

        return linePainters;
    }

}

interface LinePainter {
    void updateLines();
}

class LinePainterBottom : LinePainter {
    private float distance;
    private List<Line> lines = new List<Line>();
    private Vector3 border;
    private float spriteWidth;
    private float spriteHeight;
    private Vector3 objectPosition;
    private float lineWidthMin;
    private float lineWidthMax;
    private int numLines;
    private float speed;

    public LinePainterBottom (Vector3 objectPosition, float spriteWidth, float spriteHeight, 
                            float radious, float lineWidthMin, float lineWidthMax, int numLines,
                            float speed) {
        this.spriteWidth = spriteWidth;
        this.spriteHeight = spriteHeight;
        this.objectPosition = objectPosition;
        this.distance = radious - spriteHeight / 2;
        this.lineWidthMin = lineWidthMin;
        this.lineWidthMax = lineWidthMax;
        this.numLines = numLines;
        this.speed = speed;
        border = new Vector3(objectPosition.x, objectPosition.y - spriteHeight / 2, 0);
        createLines();
    }

    public void updateLines() {
        foreach (Line line in lines) {
            Vector3 newPos = line.getPos();
            newPos.y -= Time.deltaTime * speed;
            if (newPos.y <= border.y - distance) {
                float overflow = newPos.y - (border.y - distance);
                newPos.y = border.y + overflow;
            }
            line.setPos(newPos);
            setLineWidth(line, newPos, border);
        }
    }

    private void createLines() {
        for (int i = 0; i < numLines; i++) {
            Vector3 pos = border;
            pos.y -= (distance / (numLines) * i);
            Line line = new Line(pos, spriteWidth, Line.Orientation.HORIZONTAL);
            setLineWidth(line, pos, border);
            lines.Add(line);
        }
    }

    private void setLineWidth(Line line, Vector3 pos, Vector3 border) {
        float lineWidth = Mathf.Lerp(lineWidthMin, lineWidthMax, 1 - ((border.y - pos.y) / distance));
        line.setLineWidth(lineWidth);
    }
}

class LinePainterTop : LinePainter {
    private float distance;
    private List<Line> lines = new List<Line>();
    private Vector3 border;
    private float spriteWidth;
    private float spriteHeight;
    private Vector3 objectPosition;
    private float lineWidthMin;
    private float lineWidthMax;
    private int numLines;
    private float speed;

    public LinePainterTop(Vector3 objectPosition, float spriteWidth, float spriteHeight,
                            float radious, float lineWidthMin, float lineWidthMax, int numLines,
                            float speed) {
        this.spriteWidth = spriteWidth;
        this.spriteHeight = spriteHeight;
        this.objectPosition = objectPosition;
        this.distance = radious - spriteHeight / 2;
        this.lineWidthMin = lineWidthMin;
        this.lineWidthMax = lineWidthMax;
        this.numLines = numLines;
        this.speed = speed;
        border = new Vector3(objectPosition.x, objectPosition.y + spriteHeight / 2, 0);
        createLines();
    }

    public void updateLines() {
        foreach (Line line in lines) {
            Vector3 newPos = line.getPos();
            newPos.y += Time.deltaTime * speed;
            if (newPos.y >= border.y + distance) {
                float overflow = newPos.y - (border.y + distance);
                newPos.y = border.y + overflow;
            }
            line.setPos(newPos);
            setLineWidth(line, newPos, border);
        }
    }

    private void createLines() {
        for (int i = 0; i < numLines; i++) {
            Vector3 pos = border;
            pos.y += (distance / (numLines) * i);
            Line line = new Line(pos, spriteWidth, Line.Orientation.HORIZONTAL);
            setLineWidth(line, pos, border);
            lines.Add(line);
        }
    }

    private void setLineWidth(Line line, Vector3 pos, Vector3 border) {
        float lineWidth = Mathf.Lerp(lineWidthMin, lineWidthMax, 1 - ((pos.y - border.y) / distance));
        line.setLineWidth(lineWidth);
    }
}

class LinePainterRight : LinePainter {
    private float distance;
    private List<Line> lines = new List<Line>();
    private Vector3 border;
    private float spriteWidth;
    private float spriteHeight;
    private Vector3 objectPosition;
    private float lineWidthMin;
    private float lineWidthMax;
    private int numLines;
    private float speed;

    public LinePainterRight(Vector3 objectPosition, float spriteWidth, float spriteHeight,
                            float radious, float lineWidthMin, float lineWidthMax, int numLines,
                            float speed) {
        this.spriteWidth = spriteWidth;
        this.spriteHeight = spriteHeight;
        this.objectPosition = objectPosition;
        this.distance = radious - spriteWidth / 2;
        this.lineWidthMin = lineWidthMin;
        this.lineWidthMax = lineWidthMax;
        this.numLines = numLines;
        this.speed = speed;
        border = new Vector3(objectPosition.x + spriteWidth / 2 , objectPosition.y, 0);
        createLines();
    }

    public void updateLines() {
        foreach (Line line in lines) {
            Vector3 newPos = line.getPos();
            newPos.x += Time.deltaTime * speed;
            if (newPos.x >= border.x + distance) {
                float overflow = newPos.x - (border.x + distance);
                newPos.x = border.x + overflow;
            }
            line.setPos(newPos);
            setLineWidth(line, newPos, border);
        }
    }

    private void createLines() {
        for (int i = 0; i < numLines; i++) {
            Vector3 pos = border;
            pos.x += (distance / (numLines) * i);
            Line line = new Line(pos, spriteHeight, Line.Orientation.VERTICAL);
            setLineWidth(line, pos, border);
            lines.Add(line);
        }
    }
   
    private void setLineWidth(Line line, Vector3 pos, Vector3 border) {
        float lineWidth = Mathf.Lerp(lineWidthMin, lineWidthMax, 1-((pos.x - border.x) / distance));
        line.setLineWidth(lineWidth);
    }
}

class LinePainterLeft : LinePainter {
    private float distance;
    private List<Line> lines = new List<Line>();
    private Vector3 border;
    private float spriteWidth;
    private float spriteHeight;
    private Vector3 objectPosition;
    private float lineWidthMin;
    private float lineWidthMax;
    private int numLines;
    private float speed;

    public LinePainterLeft(Vector3 objectPosition, float spriteWidth, float spriteHeight,
                            float radious, float lineWidthMin, float lineWidthMax, int numLines,
                            float speed) {
        this.spriteWidth = spriteWidth;
        this.spriteHeight = spriteHeight;
        this.objectPosition = objectPosition;
        this.distance = radious - spriteWidth / 2;
        this.lineWidthMin = lineWidthMin;
        this.lineWidthMax = lineWidthMax;
        this.numLines = numLines;
        this.speed = speed;
        border = new Vector3(objectPosition.x - spriteWidth / 2, objectPosition.y, 0);
        createLines();
    }

    public void updateLines() {
        foreach (Line line in lines) {
            Vector3 newPos = line.getPos();
            newPos.x -= Time.deltaTime * speed;
            if (newPos.x <= border.x - distance) {
                float overflow = newPos.x - (border.x - distance);
                newPos.x = border.x + overflow;
            }
            line.setPos(newPos);
            setLineWidth(line, newPos, border);
        }
    }

    private void createLines() {
        for (int i = 0; i < numLines; i++) {
            Vector3 pos = border;
            pos.x -= (distance / (numLines) * i);
            Line line = new Line(pos, spriteHeight, Line.Orientation.VERTICAL);
            setLineWidth(line, pos, border);
            lines.Add(line);
        }
    }

    private void setLineWidth(Line line, Vector3 pos, Vector3 border) {
        float lineWidth = Mathf.Lerp(lineWidthMin, lineWidthMax, 1 - ((border.x - pos.x) / distance));
        line.setLineWidth(lineWidth);
    }
}

class Line {
    private GameObject gameObject = new GameObject();
    private LineRenderer lineRenderer;
    private float size;
    private Vector3 pos;
    private Orientation orientation;

    public enum Orientation { HORIZONTAL, VERTICAL}
    
    public Line(Vector3 pos, float size, Orientation orientation) {
        this.size = size;
        this.orientation = orientation;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        setPos(pos);
        
    }

    public Line setLineWidth(float width) {
        lineRenderer.SetWidth(width, width); return this;
    }

    public Line setPos(Vector3 pos) {
        this.pos = pos;
        if (orientation == Orientation.HORIZONTAL) {
            lineRenderer.SetPosition(0, new Vector3(pos.x - size / 2, pos.y, 0));
            lineRenderer.SetPosition(1, new Vector3(pos.x + size / 2, pos.y, 0));
        }
        else if(orientation == Orientation.VERTICAL) {
            lineRenderer.SetPosition(0, new Vector3(pos.x, pos.y + size / 2, 0));
            lineRenderer.SetPosition(1, new Vector3(pos.x, pos.y - size / 2, 0));
        }
        return this;
    }

    public Vector3 getPos() { return pos; }
}

/* private void addLineRenderer(GameObject line, float yOffset, float width) {
     LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
     lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
     lineRenderer.SetWidth(width, width);
     float xPos = gameObject.transform.position.x;
     float yPos = gameObject.transform.position.y - spriteHeight / 2 - yOffset;
     Vector3 initPos = new Vector3(xPos - spriteWidth / 2, yPos, 0);
     Vector3 finishPos = new Vector3(xPos + spriteWidth / 2, yPos, 0);
     lineRenderer.SetPosition(0, initPos);
     lineRenderer.SetPosition(1, finishPos);
 }*/



/*float sizeValue = (2.0f * Mathf.PI) / theta_scale;
int size = (int)sizeValue;
size++;
lineRenderer = gameObject.AddComponent<LineRenderer>();
lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
lineRenderer.SetWidth(0.02f, 0.02f); //thickness of line
Vector3 initPos = new Vector3(gameObject.transform.position.x - 1, gameObject.transform.position.y, 0);
Vector3 finishos = new Vector3(gameObject.transform.position.x + 1, gameObject.transform.position.y, 0);



lineRenderer.SetVertexCount(size);

Vector3 pos;
float theta = 0f;
for (int i = 0; i < size; i++) {
    theta += (2.0f * Mathf.PI * theta_scale);
    float x = radius * Mathf.Cos(theta);
    float y = radius * Mathf.Sin(theta);
    x += gameObject.transform.position.x;
    y += gameObject.transform.position.y;
    pos = new Vector3(x, y, 0);
    lineRenderer.SetPosition(i, pos);
}*/
