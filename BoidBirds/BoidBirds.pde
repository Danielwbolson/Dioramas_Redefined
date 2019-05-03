/** 

This software was written for the Orbacles project by MINN_LAB, part of the 
2017 Minneapolis Creative City Challenge and Northern Spark Arts Festival.

Copyright Daniel Keefe and Regents of the University of Minnesota, 2017.
Portions of the software are adapted from ideas and examples presented in [1,2]
and the data are derived from [3].  

REFERENCES/CREDITS:
[1] The Nature of Code by Daniel Shiffman, especially Chp 6 on automomous agents
    http://natureofcode.com/book/chapter-6-autonomous-agents/
    Examples, GPU licensed.
    
[2] Original Boids algorithm by Craig Reynolds.  Simple online description and pseudocode:
    http://www.kfish.org/boids/pseudocode.html

[3] Bird Atlas dataset extracted for MN:
    Matthews, S.N., L. R. Iverson, A.M. Prasad, A. M., and M.P. Peters. 2007-ongoing. 
    A Climate Change Atlas for 147 Bird Species of the Eastern United States [database]. 
    https://www.nrs.fs.fed.us/atlas/bird, 
    Northern Research Station, USDA Forest Service, Delaware, Ohio.
    
**/



enum BirdState {
  FLYING, GLIDING, SEEKINGNEST, NESTING;
};

enum WingState {
  FLAPPING, GLIDING, NESTING;
};


class Bird {
  PVector nestPos;
  PVector position;
  PVector velocity;
  PVector acceleration;
  float size;
  color col;
  BirdState state;
  WingState wingState;
  float tWing;
}

Bird [] birds;

float cohesionWeight = 0.001;
float separationWeight = 0.4;
float alignmentWeight = 0.25;
float avoidOrbsWeight = 0.35;
float minSpeed = 2.5;
float maxSpeed = 7.0;
float gotoNestProbability = 10000;
float leaveNestProbability = 100;


void seekTarget(int birdID, PVector targetPt) {
  PVector toTarget = PVector.sub(targetPt, birds[birdID].position);
  float dist = toTarget.mag();
  PVector desiredVel = toTarget.mult(0.05);
  if (dist < 110) {
    float alpha = map(dist, 0, 110, 0, minSpeed);
    desiredVel.normalize();
    desiredVel.mult(alpha);
    birds[birdID].acceleration = PVector.sub(desiredVel, birds[birdID].velocity);
    birds[birdID].wingState = WingState.FLAPPING;
  }
  else {
    desiredVel = PVector.add(desiredVel, calcSeparationVel(birdID));
    desiredVel.limit(minSpeed);
    PVector steeringForce = PVector.sub(desiredVel, birds[birdID].velocity);
    birds[birdID].acceleration = steeringForce.div(birds[birdID].size);  // F = MA  or A = F/M
  }
}


PVector calcCohesionVel(int birdID) {
  PVector centerOfMass = new PVector(0,0);
  int count = 0;
  for (int i=0; i<birds.length; i++) {
    if (i != birdID) {
      PVector diff = PVector.sub(birds[i].position, birds[birdID].position);
      float cutoff = 200;
      //if ((diff.mag() <= cutoff) && (PVector.dot(birds[birdID].velocity, diff) > 0.0)) {
      if (diff.mag() <= cutoff) {
        centerOfMass.add(diff);
        count++;
      }
    }
  }
  if (count > 0) {
    centerOfMass = centerOfMass.div(count);
    return centerOfMass.mult(cohesionWeight);
  }
  else {
    return centerOfMass; 
  }
}

PVector calcSeparationVel(int birdID) {
  PVector awayDir = new PVector(0,0);
  for (int i=0; i<birds.length; i++) {
    if (i != birdID) {
      float dist = birds[birdID].position.dist(birds[i].position);
      float cutoff = birds[birdID].size/2 + birds[i].size/2 + 8;
      if (dist <= cutoff) {
        PVector awayFromOther = PVector.sub(birds[birdID].position, birds[i].position);       
        awayFromOther.normalize();
        awayDir.add(awayFromOther);
      }
    }
  }
  if (awayDir.mag() > 0.0) {
    awayDir.normalize();
  } 
  return awayDir.mult(separationWeight);
}


PVector calcAlignmentVel(int birdID) {
  PVector avgV = new PVector(0,0);
  int count = 0;
  for (int i=0; i<birds.length; i++) {
    if (i != birdID) {
      PVector diff = PVector.sub(birds[i].position, birds[birdID].position);
      float cutoff = 50;
      //if ((diff.mag() <= cutoff) && (PVector.dot(birds[birdID].velocity, diff) > 0.0)) {
      if (diff.mag() <= cutoff) {
        avgV = avgV.add(birds[i].velocity);
        count++;
      }
    }
  }
  if (count > 0) {
    avgV = avgV.div(count);
    return avgV.mult(alignmentWeight);
  }
  else {
    return avgV; 
  }
}


PVector calcAvoidOrbsVel(int birdID) {
  PVector awayDir = new PVector(0,0);
  float tooClose = 250.0;
  PVector fromOrb1 = PVector.sub(birds[birdID].position, new PVector(curX, curY));
  if ((fromOrb1.mag() > 0.0) && (fromOrb1.mag() < tooClose)) {
    awayDir.add(fromOrb1.normalize());    
  }
  PVector fromOrb2 = PVector.sub(birds[birdID].position, new PVector(loX, loY));
  if ((fromOrb2.mag() > 0.0) && (fromOrb2.mag() < tooClose)) {
    awayDir.add(fromOrb2.normalize());    
  }
  PVector fromOrb3 = PVector.sub(birds[birdID].position, new PVector(hiX, hiY));
  if ((fromOrb3.mag() > 0.0) && (fromOrb3.mag() < tooClose)) {
    awayDir.add(fromOrb3.normalize());    
  }

  return awayDir.mult(avoidOrbsWeight);
}


void updateSim() {
  for (int i=0; i<birds.length; i++) {

    if (birds[i].state == BirdState.SEEKINGNEST) {
      birds[i].tWing += random(10.0*PI/(birds[i].size*birds[i].size));
      
      float dist = birds[i].position.dist(birds[i].nestPos);
      if (birds[i].position.dist(birds[i].nestPos) <= 1.0) {
        birds[i].position = birds[i].nestPos.copy();
        birds[i].state = BirdState.NESTING;
      }
      else {
        seekTarget(i, birds[i].nestPos);
        birds[i].velocity.add(birds[i].acceleration);
        if (birds[i].velocity.mag() > maxSpeed) {
          birds[i].velocity.normalize();
          birds[i].velocity.mult(maxSpeed);
        }
        birds[i].acceleration.mult(0.0);    
        birds[i].position.add(birds[i].velocity);
      }
    }
    else if (birds[i].state == BirdState.FLYING) {
      // 1/x probability of deciding to return to nest
      if (Math.round(random(0,gotoNestProbability)) == 0) {
        birds[i].state = BirdState.SEEKINGNEST; 
      }
      else {
        PVector desiredVel = new PVector(0,0);
        desiredVel = PVector.add(desiredVel, calcCohesionVel(i));
        desiredVel = PVector.add(desiredVel, calcAlignmentVel(i));
        desiredVel = PVector.add(desiredVel, calcSeparationVel(i));
        desiredVel = PVector.add(desiredVel, calcAvoidOrbsVel(i));
        if (desiredVel.mag() < minSpeed) {
          desiredVel.normalize();
          desiredVel.mult(minSpeed);
        }
        else if (desiredVel.mag() > maxSpeed) {
          desiredVel.normalize();
          desiredVel.mult(maxSpeed);
        }
        PVector steeringForce = PVector.sub(desiredVel, birds[i].velocity);
        birds[i].acceleration = steeringForce.div(birds[i].size);  // F = MA  or A = F/M
        PVector oldHeading = birds[i].velocity.copy().normalize();
        PVector newHeading = PVector.add(birds[i].velocity, birds[i].acceleration).normalize();
        if (PVector.dot(oldHeading, newHeading) > 0.5) {
          birds[i].velocity.add(birds[i].acceleration);
        }
        else {
          birds[i].velocity.add(birds[i].acceleration.mult(0.1)); 
        }
        birds[i].acceleration.mult(0.0);
    
        birds[i].position.add(birds[i].velocity);
     
        
        if (birds[i].position.x < -birds[i].size) {
          birds[i].position.x = width + birds[i].size;
        }
        else if (birds[i].position.x > width + birds[i].size) {
          birds[i].position.x = -birds[i].size;
        }
        if (birds[i].position.y < -birds[i].size) {
          birds[i].position.y = height + birds[i].size;
        }
        else if (birds[i].position.y > height + birds[i].size) {
          birds[i].position.y = -birds[i].size;
        }
      }
    }
    else if (birds[i].state == BirdState.NESTING) {
      birds[i].wingState = WingState.NESTING;
      // 1/x probability of deciding to leave the nest
      if (Math.round(random(0,leaveNestProbability)) == 0) {
        birds[i].state = BirdState.FLYING;
        birds[i].wingState = WingState.FLAPPING;
        birds[i].velocity = (new PVector(random(-1,1), random(-1,1))).normalize().mult(minSpeed);
      }
    }
    
    
    if (birds[i].wingState == WingState.FLAPPING) {
      birds[i].tWing += random(10.0*PI/(birds[i].size*birds[i].size));
      if (Math.round(random(0,2500)) == 0) {
        birds[i].wingState = WingState.GLIDING;    
      }      
    }
    else if (birds[i].wingState == WingState.GLIDING) {
      birds[i].tWing = PI/2;
      if (Math.round(random(0,2500)) == 0) {
        birds[i].wingState = WingState.FLAPPING;
      }
    }
    else {
      birds[i].tWing = 0.0;      
    }
  }
}



// defaults, overwritten in setup;
int curX = 200;
int curY = 200;
int loX = 500;
int loY = 200;
int hiX = 800;
int hiY = 200;


float rad = 28.0;
float radinc = 14.0;
int basesize = 8;
int maxsize = 14;
int [] birdsPerRow =  { 14, 19, 21, 23, 24, 23, 23 };

color [] colarray = { 
  color(255,103,0), 
  color(255,165,92),
  color(255,229,186), 
  color(255,255,255), 
  color(190,254,255), 
  color(76,255,227),
  color(0,182,183) 
};



Table table;
 
void resetFlock(int orbCtrX, int orbCtrY, String hiLoCur, int orbStartIndex) {
  float r = rad;
  float angle = 0;
  int birdcount = 0;
  int rowcount = 0;
  int orbcount = 0;
  for (TableRow row : table.rows()) { 
    int nestX = orbCtrX + round(r * cos(angle));
    int nestY = orbCtrY + round(r * sin(angle));
    float size = basesize + (maxsize-basesize)*row.getFloat("Size_0to1");
    color col;
    if (hiLoCur == "cur") {
      if (row.getFloat("Incidence0to1_cur") == -99.0) {
        col = color(0);
      }
      else {
        col = color(255);
      }
    }
    else {
      int colindex = row.getInt("IncidenceChangeCategory_" + hiLoCur) + 3;
      if (colindex >= 0) {
        col = colarray[colindex];
      }
      else {
        col = color(0);
      }
    }    
    
    PVector pos = new PVector(nestX, nestY);
    PVector ctr = new PVector(orbCtrX, orbCtrY);
    PVector posRelative = pos.copy();
    posRelative.sub(ctr);
    PVector heading = posRelative.cross(new PVector(0,0,1)).normalize();

    birds[orbStartIndex+orbcount].position = pos;
    birds[orbStartIndex+orbcount].nestPos = pos.copy();
    birds[orbStartIndex+orbcount].velocity = heading.mult(maxSpeed);
    birds[orbStartIndex+orbcount].size = size;
    birds[orbStartIndex+orbcount].col = col;
    birds[orbStartIndex+orbcount].state = BirdState.FLYING;
    birds[orbStartIndex+orbcount].wingState = WingState.FLAPPING;
    

    orbcount++;
    birdcount++;          
    if (birdcount == birdsPerRow[rowcount]) {
      r += radinc; 
      birdcount = 0;
      angle = 0.0;
      rowcount++;
    }
    else {
      angle += 2.0*PI / (float)birdsPerRow[rowcount];      
    }
  }  
}
  


void drawOrb(PGraphics p, int xcenter, int ycenter, String lohicur) {
  float r = rad;
  float angle = 0;
  int birdcount = 0;
  int rowcount = 0;
  for (TableRow row : table.rows()) {
    
    int x = round(xcenter + r * cos(angle));
    int y = round(ycenter + r * sin(angle));
    float size = basesize + (maxsize-basesize)*row.getFloat("Size_0to1");
    
    if (lohicur == "cur") {
      if (row.getFloat("Incidence0to1_cur") == -99.0) {
        p.fill(0);
      }
      else {
        p.fill(255,255,255,100);
      }
    }
    else {
      int colindex = row.getInt("IncidenceChangeCategory_" + lohicur) + 3;
      if (colindex >= 0) {
        p.fill(colarray[colindex],100);
      }
      else {
        p.fill(0);
      }
    }
    
    p.ellipse(x,y,size,size);
    
    birdcount++;          
    if (birdcount == birdsPerRow[rowcount]) {
      r += radinc; 
      birdcount = 0;
      angle = 0.0;
      rowcount++;
    }
    else {
      angle += 2.0*PI / (float)birdsPerRow[rowcount];      
    }
  }
}


PGraphics offscreen;
boolean calibrating = false;
int corner = 0;
PVector [] corners;




void setup() {
  //size(2560, 768, P2D);
  fullScreen(P2D, SPAN);
  offscreen = createGraphics(width,height);
  
  curX = width / 2;
  curY = round(0.6 * (float)height);
  loX = 200;
  loY = round(0.4 * (float)height);;
  hiX = width - 200;
  hiY = round(0.4 * (float)height);
  
  println(curX + " " + curY);
  println(loX + " " + loY);
  println(hiX + " " + hiY);

  textureMode(NORMAL);
  corners = new PVector[8];
  corners[0] = new PVector(0, 0);
  corners[1] = new PVector(width/2, 0);
  corners[2] = new PVector(width/2, height);
  corners[3] = new PVector(0, height);
  
  corners[4] = new PVector(width/2, 0);
  corners[5] = new PVector(width, 0);
  corners[6] = new PVector(width, height);
  corners[7] = new PVector(width/2, height);

  // loads in per species data from csv file
  table = loadTable("birddata.csv", "header");  
  for (TableRow row : table.rows()) {
    int id = row.getInt("SpeciesNum");
    String species = row.getString("SciName").trim();
    String name = row.getString("CommonName").trim();
    println(name + " (" + species + ") has an ID of " + id);
  }

  birds = new Bird[table.getRowCount()*3];
  for (int i=0; i<birds.length; i++) {
    birds[i] = new Bird();
    birds[i].position = new PVector(500,500);
    birds[i].nestPos = new PVector(500,500);
    birds[i].velocity = new PVector(0,0);
    birds[i].acceleration = new PVector(0,0);
    birds[i].size = 1.0;
    birds[i].col = colarray[0];
    birds[i].state = BirdState.FLYING;
    birds[i].wingState = WingState.FLAPPING;
    birds[i].tWing = 0.0;
  }

  resetFlock(curX, curY, "cur", 0);
  resetFlock(loX, loY, "lo", table.getRowCount());
  resetFlock(hiX, hiY, "hi", table.getRowCount()*2);
}



void draw() {
  
  updateSim();
  
  offscreen.beginDraw();
  
  offscreen.background(0);
  offscreen.stroke(100);
  drawOrb(offscreen, curX, curY, "cur");
  drawOrb(offscreen, loX, loY, "lo");
  drawOrb(offscreen, hiX, hiY, "hi");
  
  offscreen.stroke(200);
  for (int i=0; i<birds.length; i++) {
    offscreen.fill(birds[i].col);
    offscreen.pushMatrix();
    offscreen.translate(birds[i].position.x, birds[i].position.y);
    offscreen.rotate(birds[i].velocity.heading() + PI/2);
    offscreen.arc(0, 0, birds[i].size/2 + 2*birds[i].size*abs(sin(birds[i].tWing)), birds[i].size*2, -PI, 0);
    offscreen.popMatrix();
  }
  
  offscreen.endDraw();
  
    // draw each projector's buffer to a quad to the screen
  background(0);
  noStroke();
  
  beginShape();
  texture(offscreen);
  vertex(corners[0].x, corners[0].y, 0, 0);
  vertex(corners[1].x, corners[1].y, 0.5, 0);
  vertex(corners[2].x, corners[2].y, 0.5, 1);
  vertex(corners[3].x, corners[3].y, 0, 1);
  endShape();
  
  beginShape();
  texture(offscreen);
  vertex(corners[4].x, corners[4].y, 0.5, 0);
  vertex(corners[5].x, corners[5].y, 1, 0);
  vertex(corners[6].x, corners[6].y, 1, 1);
  vertex(corners[7].x, corners[7].y, 0.5, 1);
  endShape();
  
  if (calibrating) {
    stroke(255);
    strokeWeight(2);
    noFill();
    
    beginShape();
    vertex(corners[0].x, corners[0].y, 0, 0);
    vertex(corners[1].x, corners[1].y, 1, 0);
    vertex(corners[2].x, corners[2].y, 1, 1);
    vertex(corners[3].x, corners[3].y, 0, 1);
    vertex(corners[0].x, corners[0].y, 0, 0);
    endShape();
  
    beginShape();
    vertex(corners[4].x, corners[4].y, 0, 0);
    vertex(corners[5].x, corners[5].y, 1, 0);
    vertex(corners[6].x, corners[6].y, 1, 1);
    vertex(corners[7].x, corners[7].y, 0, 1);
    vertex(corners[4].x, corners[4].y, 0, 0);
    endShape();
    
    stroke(255, 0, 0);
    strokeWeight(6);
    ellipse(corners[corner].x, corners[corner].y, 20, 20);
  }
  
  
  // This is intentionally inside the draw call so that you can
  // hold down an arrow key and move a corner rather than tapping
  // it hundreds of times.
  if (keyPressed) {
    if ((key == CODED) && (keyCode == UP)) {
      corners[corner].y--;
    }
    else if ((key == CODED) && (keyCode == DOWN)) {
      corners[corner].y++;    
    }
    else if ((key == CODED) && (keyCode == LEFT)) {
      corners[corner].x--;    
    }
    else if ((key == CODED) && (keyCode == RIGHT)) {
      corners[corner].x++;    
    }
  }  
}


void keyPressed() {
  if (key == 'r' || key == 'R') {
    resetFlock(curX, curY, "cur", 0);
    resetFlock(loX, loY, "lo", table.getRowCount());
    resetFlock(hiX, hiY, "hi", table.getRowCount()*2);
  }
  else if (key == 'c' || key == 'C') {
    calibrating = !calibrating;
  }
  else if ((key >= '1') && (key <= '8')) {
    corner = int(key) - 49;
    println("Calibrating corner " + key);
  }
}