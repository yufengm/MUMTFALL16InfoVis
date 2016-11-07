var cloud;

var root;
function setup() {
    frameRate(30);
    createCanvas(windowWidth, windowHeight);
    background(255);
    cloud = new WordCloud();
}

function draw() {
    background(255);
    cloud.update();

    console.log(cloud.wordNodes.length);
    for (var i = 0; i < cloud.wordNodes.length; i++) {
        fill(0);
        noStroke();
        var node = cloud.wordNodes[i];
        textSize(node.weight);
        text(node.txt, node.x, node.y+node.h);
    }
    //for (var i = 0; i < cloud.wordNodes.length; i++) {
    //    var node1 = cloud.wordNodes[i];
    //    rect(node1.x,node1.y, node1.w, node1.h);
    //}
}

function mousePressed() {
    cloud.step = 10;
    var node = new Node();
    node.txt = "test" + cloud.wordNodes.length;
    node.group = "one";
    node.x = mouseX;
    node.y = mouseY;
    node.attrX = mouseX;
    node.attrY = mouseY;
    node.weight = random(30, 50);
    var font = node.weight + "px Arial";
    var tsize = getTextSize(node.txt, font, node.weight);
    node.w = tsize.w;
    node.h = tsize.h;
    cloud.push(node);
}


