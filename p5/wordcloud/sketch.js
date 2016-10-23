var cars = ["Banana", "Orange", "Apple", "Mango"];
function setup() {
  createCanvas(windowWidth, windowHeight);
  background(0);
}

function draw() {
  background(0);
  fill(255,100,0);
  textSize(70);
  for (i = 0; i < cars.length; i++) {
    text(cars[i], random(0,windowWidth),random(0,windowHeight));
  }
}