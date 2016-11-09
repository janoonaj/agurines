using UnityEngine;
using System.Collections.Generic;
using System;

public class MagneticBehaviour : MonoBehaviour {
    public float magneticDistance;
    public Effector2D effectorGUI;
    public Side[] sides;
    public float[] times;
    public MagneticType[] magneticTypes;
    private int magneticStateIndex = 0;
    private Clock clock;
    private EffectorEmbed effector;
    private List<LinePainter> linePainters;
    private float lineWidthMax = 0.06f;
    private float lineWidthMin = 0.02f;
    private float effectorForce;

    private const float SPEED = 1f;

    public enum Side{ TOP, BOTTOM, RIGHT, LEFT}
    public enum MagneticType { ATTRACT, REPEL, NONE}


    void Start() {
        checkGUIData();
        effector = new EffectorEmbed(effectorGUI);
        effectorForce = Math.Abs(effector.getForceMagnitude());
        updateMagneticForce();
        linePainters = createLinePainters();
        if (times.Length > 0)
            clock = new Clock(currentTime());
    }

    void Update() {
        if(clock != null) {
            if(clock.update(Time.deltaTime)) {
                magneticStateIndex++;
                if (magneticStateIndex >= times.Length) magneticStateIndex = 0;
                updateMagneticForce();
                foreach (LinePainter linePainter in linePainters) {
                    linePainter.reset(currentMagneticState());
                }

                if (currentMagneticState() == MagneticType.NONE) {
                    foreach (LinePainter linePainter in linePainters) {
                            linePainter.hide();
                        }
                } else {
                    foreach (LinePainter linePainter in linePainters) {
                            linePainter.show();
                        }
                }

                clock = new Clock(currentTime());
            }
        }

        if (currentMagneticState() == MagneticType.NONE)
            return;
        
        foreach(LinePainter linePainter in linePainters) {
            linePainter.updateLines();
        }
    }

    private void checkGUIData() {
        if((times.Length > 0) && (times.Length != magneticTypes.Length))
            throw (new System.Exception("Number of times and number of magnetic states must coincide. (MagneticBehaviour.cs)"));
    }

    private void updateMagneticForce() {
        if (currentMagneticState() == MagneticType.REPEL) {
            effector.setForceMagnitude(effectorForce);
        }
        else if (currentMagneticState() == MagneticType.ATTRACT) {
            effector.setForceMagnitude(-effectorForce);
        }
        else if (currentMagneticState() == MagneticType.NONE) {
            effector.setForceMagnitude(0f);
        }
    }

    private MagneticType currentMagneticState() {
        return magneticTypes[magneticStateIndex];
    }

    private float currentTime() {
        return times[magneticStateIndex];
    }


    private List<LinePainter> createLinePainters() {
        List<LinePainter> linePainters = new List<LinePainter>();
        // num_lines Not 100% accurate, radious is not the magnetic distance (it's taking the center, 
        // but should start on the side of the floor. distance = radious - spriteHeight / 2)
        int num_lines = (int)Mathf.Max(magneticDistance * 4f, 4f);
        float spriteWidth = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        float spriteHeight = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
        foreach (Side side in sides) {
            if(side == Side.BOTTOM) {
                linePainters.Add(new LinePainterBottom(gameObject.transform.position, spriteWidth,
                                                        spriteHeight, magneticDistance, 
                                                        lineWidthMin, lineWidthMax, num_lines, SPEED, currentMagneticState()));
            } 
            else if (side == Side.TOP) {
                linePainters.Add(new LinePainterTop(gameObject.transform.position, spriteWidth,
                                                        spriteHeight, magneticDistance,
                                                        lineWidthMin, lineWidthMax, num_lines, SPEED, currentMagneticState()));
            }
            else if (side == Side.RIGHT) {
                linePainters.Add(new LinePainterRight(gameObject.transform.position, spriteWidth,
                                                        spriteHeight, magneticDistance,
                                                        lineWidthMin, lineWidthMax, num_lines, SPEED, currentMagneticState()));
            }
            else if (side == Side.LEFT) {
                linePainters.Add(new LinePainterLeft(gameObject.transform.position, spriteWidth,
                                                        spriteHeight, magneticDistance,
                                                        lineWidthMin, lineWidthMax, num_lines, SPEED, currentMagneticState()));
            }
        }

        return linePainters;
    }

}

abstract class LinePainter {
    protected List<Line> lines = new List<Line>();
    protected float distance;
    protected Vector3 border;
    protected float spriteWidth;
    protected float spriteHeight;
    protected Vector3 objectPosition;
    protected float lineWidthMin;
    protected float lineWidthMax;
    protected int numLines;
    protected float speed;
    protected MagneticBehaviour.MagneticType magneticType;


    abstract public void updateLines();
    abstract protected Vector3 calculatePos(int lineIndex);
    public void hide() {
        foreach (Line line in lines) {
            line.hide();
        }
    }
    public void show() {
        foreach (Line line in lines) {
            line.show();
        }
    }

    public void reset(MagneticBehaviour.MagneticType magneticType) {
        this.magneticType = magneticType;
        foreach (Line line in lines) {
            line.reset();
        }
    }
}

class LinePainterBottom : LinePainter {

    public LinePainterBottom(Vector3 objectPosition, float spriteWidth, float spriteHeight,
                            float radious, float lineWidthMin, float lineWidthMax, int numLines,
                            float speed, MagneticBehaviour.MagneticType magneticType) {
        this.spriteWidth = spriteWidth;
        this.spriteHeight = spriteHeight;
        this.objectPosition = objectPosition;
        this.distance = radious - spriteHeight / 2;
        this.lineWidthMin = lineWidthMin;
        this.lineWidthMax = lineWidthMax;
        this.numLines = numLines;
        this.speed = speed;
        this.magneticType = magneticType;
        border = new Vector3(objectPosition.x, objectPosition.y - spriteHeight / 2, 0);
        createLines();
    }

    public override void updateLines() {
        foreach (Line line in lines) {
            Vector3 newPos = line.getPos();
            if (magneticType == MagneticBehaviour.MagneticType.REPEL) {
                newPos.y -= Time.deltaTime * speed;
                if (newPos.y <= border.y - distance) {
                    float overflow = newPos.y - (border.y - distance);
                    newPos.y = border.y + overflow;
                }
            }
            else {
                newPos.y += Time.deltaTime * speed;
                if (newPos.y >= border.y) {
                    float overflow = newPos.y - border.y;
                    newPos.y = (border.y - distance) + overflow;
                }
            }
            line.setPos(newPos);
            line.setLineWidth(calculateWidth(newPos, border));
        }
    }

    private void createLines() {
        for (int i = 0; i < numLines; i++) {
            Vector3 pos = calculatePos(i);
            Line line = new Line(pos, spriteWidth, calculateWidth(pos, border), Line.Orientation.HORIZONTAL);
            lines.Add(line);
        }
    }

    protected override Vector3 calculatePos(int lineIndex) {
        Vector3 pos = border;
        pos.y -= (distance / (numLines) * lineIndex);
        return pos;
    }

    private float calculateWidth(Vector3 pos, Vector3 border) {
        return Mathf.Lerp(lineWidthMin, lineWidthMax, 1 - ((border.y - pos.y) / distance));
    }
}

class LinePainterTop : LinePainter {

    public LinePainterTop(Vector3 objectPosition, float spriteWidth, float spriteHeight,
                            float radious, float lineWidthMin, float lineWidthMax, int numLines,
                            float speed, MagneticBehaviour.MagneticType magneticType) {
        this.spriteWidth = spriteWidth;
        this.spriteHeight = spriteHeight;
        this.distance = radious - spriteHeight / 2;
        this.lineWidthMin = lineWidthMin;
        this.lineWidthMax = lineWidthMax;
        this.numLines = numLines;
        this.speed = speed;
        this.magneticType = magneticType;
        border = new Vector3(objectPosition.x, objectPosition.y + spriteHeight / 2, 0);
        createLines();
    }

    public override void updateLines() {
        foreach (Line line in lines) {
            Vector3 newPos = line.getPos();
            if (magneticType == MagneticBehaviour.MagneticType.REPEL) {
                newPos.y += Time.deltaTime * speed;
                if (newPos.y >= border.y + distance) {
                    float overflow = newPos.y - (border.y + distance);
                    newPos.y = border.y + overflow;
                }
            }
            else {
                newPos.y -= Time.deltaTime * speed;
                if (newPos.y <= border.y) {
                    float overflow = border.y - newPos.y;
                    newPos.y = (border.y + distance) - overflow;
                }
            }
            line.setPos(newPos);
            line.setLineWidth(calculateWidth(newPos, border));
        }
    }

    private void createLines() {
        for (int i = 0; i < numLines; i++) {
            Vector3 pos = calculatePos(i);
            Line line = new Line(pos, spriteWidth, calculateWidth(pos, border), Line.Orientation.HORIZONTAL);
            lines.Add(line);
        }
    }

    protected override Vector3 calculatePos(int lineIndex) {
        Vector3 pos = border;
        pos.y += (distance / (numLines) * lineIndex);
        return pos;
    }

    private float calculateWidth(Vector3 pos, Vector3 border) {
        return Mathf.Lerp(lineWidthMin, lineWidthMax, 1 - ((pos.y - border.y) / distance));
    }
}

class LinePainterRight : LinePainter {
    public LinePainterRight(Vector3 objectPosition, float spriteWidth, float spriteHeight,
                            float radious, float lineWidthMin, float lineWidthMax, int numLines,
                            float speed, MagneticBehaviour.MagneticType magneticType) {
        this.spriteWidth = spriteWidth;
        this.spriteHeight = spriteHeight;
        this.objectPosition = objectPosition;
        this.distance = radious - spriteWidth / 2;
        this.lineWidthMin = lineWidthMin;
        this.lineWidthMax = lineWidthMax;
        this.numLines = numLines;
        this.speed = speed;
        this.magneticType = magneticType;
        border = new Vector3(objectPosition.x + spriteWidth / 2 , objectPosition.y, 0);
        createLines();
    }

    public override void updateLines() {
        foreach (Line line in lines) {
            Vector3 newPos = line.getPos();
            if (magneticType == MagneticBehaviour.MagneticType.REPEL) {
                newPos.x += Time.deltaTime * speed;
                if (newPos.x >= border.x + distance) {
                    float overflow = newPos.x - (border.x + distance);
                    newPos.x = border.x + overflow;
                }
            } else {
                newPos.x -= Time.deltaTime * speed;
                if (newPos.x <= border.x) {
                    float overflow = border.x - newPos.x;
                    newPos.x = (border.x + distance) - overflow;
                }
            }
            line.setPos(newPos);
            line.setLineWidth(calculateWidth(newPos, border));
        }
    }

    private void createLines() {
        for (int i = 0; i < numLines; i++) {
            Vector3 pos = calculatePos(i);
            Line line = new Line(pos, spriteHeight, calculateWidth(pos, border), Line.Orientation.VERTICAL);
            lines.Add(line);
        }
    }

    protected override Vector3 calculatePos(int lineIndex) {
        Vector3 pos = border;
        pos.x += (distance / (numLines) * lineIndex);
        return pos;
    }

    private float calculateWidth(Vector3 pos, Vector3 border) {
        return Mathf.Lerp(lineWidthMin, lineWidthMax, 1 - ((pos.x - border.x) / distance));
    }
}

class LinePainterLeft : LinePainter {
    public LinePainterLeft(Vector3 objectPosition, float spriteWidth, float spriteHeight,
                            float radious, float lineWidthMin, float lineWidthMax, int numLines,
                            float speed, MagneticBehaviour.MagneticType magneticType) {
        this.spriteWidth = spriteWidth;
        this.spriteHeight = spriteHeight;
        this.objectPosition = objectPosition;
        this.distance = radious - spriteWidth / 2;
        this.lineWidthMin = lineWidthMin;
        this.lineWidthMax = lineWidthMax;
        this.numLines = numLines;
        this.speed = speed;
        this.magneticType = magneticType;
        border = new Vector3(objectPosition.x - spriteWidth / 2, objectPosition.y, 0);
        createLines();
    }

    public override void updateLines() {
        foreach (Line line in lines) {
            Vector3 newPos = line.getPos();
            if (magneticType == MagneticBehaviour.MagneticType.REPEL) {
                newPos.x -= Time.deltaTime * speed;
                if (newPos.x <= border.x - distance) {
                    float overflow = newPos.x - (border.x - distance);
                    newPos.x = border.x + overflow;
                }
            } else {
                newPos.x += Time.deltaTime * speed;
                if (newPos.x >= border.x) {
                    float overflow = newPos.x - border.x;
                    newPos.x = border.x - distance + overflow;
                }
            }
            line.setPos(newPos);
            line.setLineWidth(calculateWidth(newPos, border));
        }
    }

    private void createLines() {
        for (int i = 0; i < numLines; i++) {
            Vector3 pos = calculatePos(i);
            Line line = new Line(pos, spriteHeight, calculateWidth(pos, border), Line.Orientation.VERTICAL);
            lines.Add(line);
        }
    }

    protected override Vector3 calculatePos(int lineIndex) {
        Vector3 pos = border;
        pos.x -= (distance / (numLines) * lineIndex);
        return pos;
    }

    private float calculateWidth(Vector3 pos, Vector3 border) {
        return Mathf.Lerp(lineWidthMin, lineWidthMax, 1 - ((border.x - pos.x) / distance));
    }
}

class Line {
    private LineRenderer lineRenderer;
    private float size;
    private Vector3 pos;
    private Orientation orientation;
    private GameObject gameObject = new GameObject();
    private float spriteHeight;
    private Vector3 startPos;
    private float startLineWidth;

    public enum Orientation { HORIZONTAL, VERTICAL}
    
    /*public Line(Vector3 pos, float size, Orientation orientation) {
        this.size = size;
        this.orientation = orientation;
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        setPos(pos);
        
    }*/

    public Line(Vector3 pos, float size, float lineWidth, Orientation orientation) {
        this.size = size;
        this.orientation = orientation;
        this.startPos = pos;
        this.startLineWidth = lineWidth;
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

    public void hide() {
        lineRenderer.enabled = false;
    }

    public void show() {
        lineRenderer.enabled = true;
    }

    public void reset() {
        setPos(startPos);
        setLineWidth(startLineWidth);
    }
}

class EffectorEmbed {
    private Effector2D effector;

    public EffectorEmbed(Effector2D effector) {
        this.effector = effector;
    }

    public float getForceMagnitude() {
        if (effector is PointEffector2D) {
            return ((PointEffector2D)effector).forceMagnitude;
        }
        else if (effector is AreaEffector2D) {
            return ((AreaEffector2D)effector).forceMagnitude;
        }
        throw (new System.Exception("Effector should be point or area. (MagneticBehaviour.cs)"));
    }

    public void setForceMagnitude(float force) {
        if (effector is PointEffector2D) {
            ((PointEffector2D)effector).forceMagnitude = force;
        }
        else if (effector is AreaEffector2D) {
            ((AreaEffector2D)effector).forceMagnitude = force;
        }
        else
            throw (new System.Exception("Effector should be point or area. (MagneticBehaviour.cs)"));
    }
}