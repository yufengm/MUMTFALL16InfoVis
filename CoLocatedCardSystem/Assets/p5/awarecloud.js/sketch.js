var fruits = ["Banana", "Orange", "Apple", "Mango"];
var frame=0;
function setup() {
    createCanvas(windowWidth, windowHeight);
    background(0);
    initData(0,"strawberry");
    frameRate(10);
}

function draw() {
    background(0);
    fill(255, 100, 0);
    textSize(70);
    frame=(frame+1)%60;
    
    for (i = 0; i < fruits.length; i++) {
        rotate(PI*frame/30);
        translate(-100*(i+1),-100*(i+1))
        text(fruits[i], 0, 0);
    }
}

function initData(index,param) {
    fruits[index] = param;
}