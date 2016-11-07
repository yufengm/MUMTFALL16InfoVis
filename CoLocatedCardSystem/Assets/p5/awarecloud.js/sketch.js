var cloud;

var root;
function setup() {
    frameRate(30);
    createCanvas(windowWidth, windowHeight);
    background(255);
    cloud = new WordCloud();
    root = new Node();
    root.txt = "root";
    root.group = "one";
    root.x = windowWidth / 2;
    root.y = windowHeight / 2;
    root.attrX = windowWidth / 3;
    root.attrY = windowHeight / 2;
    root.weight = 50;
    var font = root.weight + "px Arial";
    var tsize = getTextSize(root.txt, font, root.weight);
    root.w = tsize.w;
    root.h = tsize.h;
    cloud.push(root);

    for (var i = 0; i < 20; i++) {
        var node = new Node();
        node.txt = "test" + i;
        node.group = "one";
        node.x = windowWidth / 2 + random(10) - 5;
        node.y = windowHeight / 2 + random(10) - 5;
        node.attrX = windowWidth / 3;
        node.attrY = windowHeight / 2;
        node.weight = random(10,50);
        var font = node.weight + "px Arial";
        var tsize = getTextSize(node.txt, font, node.weight);
        node.w = tsize.w;
        node.h = tsize.h;
        root.connections.push(node);
        cloud.push(node);
    }
    sort(cloud.wordNodes);
}

function draw() {
    cloud.update();
    background(0);
    for (var i = 0; i < cloud.wordNodes.length; i++) {
        fill(255);
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
    node.x = windowWidth / 2 + random(10) - 5;
    node.y = windowHeight / 2 + random(10) - 5;
    node.attrX = windowWidth / 3;
    node.attrY = windowHeight / 2;
    node.weight = random(10, 50);
    var font = node.weight + "px Arial";
    var tsize = getTextSize(node.txt, font, node.weight);
    node.w = tsize.w;
    node.h = tsize.h;
    root.connections.push(node);
    cloud.push(node);
}


