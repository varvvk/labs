(function (cjs, an) {

var p; // shortcut to reference prototypes
var lib={};var ss={};var img={};
lib.ssMetadata = [];


(lib.AnMovieClip = function(){
	this.actionFrames = [];
	this.ignorePause = false;
	this.currentSoundStreamInMovieclip;
	this.soundStreamDuration = new Map();
	this.streamSoundSymbolsList = [];

	this.gotoAndPlayForStreamSoundSync = function(positionOrLabel){
		cjs.MovieClip.prototype.gotoAndPlay.call(this,positionOrLabel);
	}
	this.gotoAndPlay = function(positionOrLabel){
		this.clearAllSoundStreams();
		var pos = this.timeline.resolve(positionOrLabel);
		if (pos != null) { this.startStreamSoundsForTargetedFrame(pos); }
		cjs.MovieClip.prototype.gotoAndPlay.call(this,positionOrLabel);
	}
	this.play = function(){
		this.clearAllSoundStreams();
		this.startStreamSoundsForTargetedFrame(this.currentFrame);
		cjs.MovieClip.prototype.play.call(this);
	}
	this.gotoAndStop = function(positionOrLabel){
		cjs.MovieClip.prototype.gotoAndStop.call(this,positionOrLabel);
		this.clearAllSoundStreams();
	}
	this.stop = function(){
		cjs.MovieClip.prototype.stop.call(this);
		this.clearAllSoundStreams();
	}
	this.startStreamSoundsForTargetedFrame = function(targetFrame){
		for(var index=0; index<this.streamSoundSymbolsList.length; index++){
			if(index <= targetFrame && this.streamSoundSymbolsList[index] != undefined){
				for(var i=0; i<this.streamSoundSymbolsList[index].length; i++){
					var sound = this.streamSoundSymbolsList[index][i];
					if(sound.endFrame > targetFrame){
						var targetPosition = Math.abs((((targetFrame - sound.startFrame)/lib.properties.fps) * 1000));
						var instance = playSound(sound.id);
						var remainingLoop = 0;
						if(sound.offset){
							targetPosition = targetPosition + sound.offset;
						}
						else if(sound.loop > 1){
							var loop = targetPosition /instance.duration;
							remainingLoop = Math.floor(sound.loop - loop);
							if(targetPosition == 0){ remainingLoop -= 1; }
							targetPosition = targetPosition % instance.duration;
						}
						instance.loop = remainingLoop;
						instance.position = Math.round(targetPosition);
						this.InsertIntoSoundStreamData(instance, sound.startFrame, sound.endFrame, sound.loop , sound.offset);
					}
				}
			}
		}
	}
	this.InsertIntoSoundStreamData = function(soundInstance, startIndex, endIndex, loopValue, offsetValue){ 
 		this.soundStreamDuration.set({instance:soundInstance}, {start: startIndex, end:endIndex, loop:loopValue, offset:offsetValue});
	}
	this.clearAllSoundStreams = function(){
		this.soundStreamDuration.forEach(function(value,key){
			key.instance.stop();
		});
 		this.soundStreamDuration.clear();
		this.currentSoundStreamInMovieclip = undefined;
	}
	this.stopSoundStreams = function(currentFrame){
		if(this.soundStreamDuration.size > 0){
			var _this = this;
			this.soundStreamDuration.forEach(function(value,key,arr){
				if((value.end) == currentFrame){
					key.instance.stop();
					if(_this.currentSoundStreamInMovieclip == key) { _this.currentSoundStreamInMovieclip = undefined; }
					arr.delete(key);
				}
			});
		}
	}

	this.computeCurrentSoundStreamInstance = function(currentFrame){
		if(this.currentSoundStreamInMovieclip == undefined){
			var _this = this;
			if(this.soundStreamDuration.size > 0){
				var maxDuration = 0;
				this.soundStreamDuration.forEach(function(value,key){
					if(value.end > maxDuration){
						maxDuration = value.end;
						_this.currentSoundStreamInMovieclip = key;
					}
				});
			}
		}
	}
	this.getDesiredFrame = function(currentFrame, calculatedDesiredFrame){
		for(var frameIndex in this.actionFrames){
			if((frameIndex > currentFrame) && (frameIndex < calculatedDesiredFrame)){
				return frameIndex;
			}
		}
		return calculatedDesiredFrame;
	}

	this.syncStreamSounds = function(){
		this.stopSoundStreams(this.currentFrame);
		this.computeCurrentSoundStreamInstance(this.currentFrame);
		if(this.currentSoundStreamInMovieclip != undefined){
			var soundInstance = this.currentSoundStreamInMovieclip.instance;
			if(soundInstance.position != 0){
				var soundValue = this.soundStreamDuration.get(this.currentSoundStreamInMovieclip);
				var soundPosition = (soundValue.offset?(soundInstance.position - soundValue.offset): soundInstance.position);
				var calculatedDesiredFrame = (soundValue.start)+((soundPosition/1000) * lib.properties.fps);
				if(soundValue.loop > 1){
					calculatedDesiredFrame +=(((((soundValue.loop - soundInstance.loop -1)*soundInstance.duration)) / 1000) * lib.properties.fps);
				}
				calculatedDesiredFrame = Math.floor(calculatedDesiredFrame);
				var deltaFrame = calculatedDesiredFrame - this.currentFrame;
				if((deltaFrame >= 0) && this.ignorePause){
					cjs.MovieClip.prototype.play.call(this);
					this.ignorePause = false;
				}
				else if(deltaFrame >= 2){
					this.gotoAndPlayForStreamSoundSync(this.getDesiredFrame(this.currentFrame,calculatedDesiredFrame));
				}
				else if(deltaFrame <= -2){
					cjs.MovieClip.prototype.stop.call(this);
					this.ignorePause = true;
				}
			}
		}
	}
}).prototype = p = new cjs.MovieClip();
// symbols:



(lib.stop = function(mode,startPosition,loop,reversed) {
if (loop == null) { loop = true; }
if (reversed == null) { reversed = false; }
	var props = new Object();
	props.mode = mode;
	props.startPosition = startPosition;
	props.labels = {};
	props.loop = loop;
	props.reversed = reversed;
	cjs.MovieClip.apply(this,[props]);

	// Слой_1
	this.shape = new cjs.Shape();
	this.shape.graphics.f().s("#006600").ss(0.1,1,1).p("AutlTIdbAAQBaAABAA/QBABAAABaIAAD1QAABahAA/QhABAhaAAI9bAAQhaAAhAhAQhAg/AAhaIAAj1QAAhaBAhAQBAg/BaAAg");
	this.shape.setTransform(0,0.025);

	this.shape_1 = new cjs.Shape();
	this.shape_1.graphics.f("#CC3333").s().p("AutFUQhaAAhAhAQg/g/AAhaIAAj1QAAhaA/hAQBAg/BaAAIdbAAQBaAAA/A/QBBBAAABaIAAD1QAABahBA/Qg/BAhaAAg");
	this.shape_1.setTransform(0,0.025);

	this.timeline.addTween(cjs.Tween.get({}).to({state:[{t:this.shape_1},{t:this.shape}]}).wait(1));

	this._renderFirstFrame();

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-116.9,-35,233.9,70.1);


(lib.start = function(mode,startPosition,loop,reversed) {
if (loop == null) { loop = true; }
if (reversed == null) { reversed = false; }
	var props = new Object();
	props.mode = mode;
	props.startPosition = startPosition;
	props.labels = {};
	props.loop = loop;
	props.reversed = reversed;
	cjs.MovieClip.apply(this,[props]);

	// Слой_1
	this.shape = new cjs.Shape();
	this.shape.graphics.f().s("#006600").ss(0.1,1,1).p("AutlTIdbAAQBaAABAA/QBABAAABaIAAD1QAABahAA/QhABAhaAAI9bAAQhaAAhAhAQhAg/AAhaIAAj1QAAhaBAhAQBAg/BaAAg");
	this.shape.setTransform(0,0.025);

	this.shape_1 = new cjs.Shape();
	this.shape_1.graphics.f("#99CC33").s().p("AutFUQhaAAhAhAQg/g/AAhaIAAj1QAAhaA/hAQBAg/BaAAIdbAAQBaAAA/A/QBBBAAABaIAAD1QAABahBA/Qg/BAhaAAg");
	this.shape_1.setTransform(0,0.025);

	this.timeline.addTween(cjs.Tween.get({}).to({state:[{t:this.shape_1},{t:this.shape}]}).wait(1));

	this._renderFirstFrame();

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-116.9,-35,233.9,70.1);


(lib._return = function(mode,startPosition,loop,reversed) {
if (loop == null) { loop = true; }
if (reversed == null) { reversed = false; }
	var props = new Object();
	props.mode = mode;
	props.startPosition = startPosition;
	props.labels = {};
	props.loop = loop;
	props.reversed = reversed;
	cjs.MovieClip.apply(this,[props]);

	// Слой_1
	this.shape = new cjs.Shape();
	this.shape.graphics.f().s("#006600").ss(0.1,1,1).p("AutlTIdbAAQBaAABAA/QBABAAABaIAAD1QAABahAA/QhABAhaAAI9bAAQhaAAhAhAQhAg/AAhaIAAj1QAAhaBAhAQBAg/BaAAg");
	this.shape.setTransform(0,0.025);

	this.shape_1 = new cjs.Shape();
	this.shape_1.graphics.f("#3399CC").s().p("AutFUQhaAAhAhAQg/g/AAhaIAAj1QAAhaA/hAQBAg/BaAAIdbAAQBaAAA/A/QBBBAAABaIAAD1QAABahBA/Qg/BAhaAAg");
	this.shape_1.setTransform(0,0.025);

	this.timeline.addTween(cjs.Tween.get({}).to({state:[{t:this.shape_1},{t:this.shape}]}).wait(1));

	this._renderFirstFrame();

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-116.9,-35,233.9,70.1);


(lib.тело = function(mode,startPosition,loop,reversed) {
if (loop == null) { loop = true; }
if (reversed == null) { reversed = false; }
	var props = new Object();
	props.mode = mode;
	props.startPosition = startPosition;
	props.labels = {};
	props.loop = loop;
	props.reversed = reversed;
	cjs.MovieClip.apply(this,[props]);

	// Слой_1
	this.shape = new cjs.Shape();
	this.shape.graphics.f().s("#000000").ss(1,1,1).p("AEWiEQAAAcgUAUQgUAUgcAAQgcAAgUgUQgTgUAAgcQAAgcATgUQAUgTAcAAQAcAAAUATQAUAUAAAcgAHgAAQAADHiMCMQhHBHhWAjQhUAjhjAAQhMAAhEgVQhsghhXhXQiMiMAAjHQAAjGCMiNQCNiMDGAAQDHAACNCMQCMCNAADGgAiQiEQAAAcgUAUQgUAUgcAAQgcAAgUgUQgTgUAAgcQAAgcATgUQAUgTAcAAQAcAAAUATQAUAUAAAcg");
	this.shape.setTransform(-3,-158.95);

	this.shape_1 = new cjs.Shape();
	this.shape_1.graphics.f("#000000").s().p("AoVVTQjdm0AAppQAApoDdm1QCbkxDHhbQBEAUBNAAQBhAABUgiQDbBQCoFKQDdG1AAJoQAAJpjdG0QjdG1k5AAQk4AAjdm1gAB/6UQgUgTAAgcQAAgcAUgUQATgUAdAAQAbAAAUAUQAUAUAAAcQAAAcgUATQgUAUgbAAQgdAAgTgUgAkn6UQgTgTAAgcQAAgcATgUQAUgUAcAAQAcAAATAUQAVAUAAAcQAAAcgVATQgTAUgcAAQgcAAgUgUg");
	this.shape_1.setTransform(0.55,1.025);

	this.timeline.addTween(cjs.Tween.get({}).to({state:[{t:this.shape_1},{t:this.shape}]}).wait(1));

	this._renderFirstFrame();

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-74.9,-207.9,151,388.9);


(lib.лапа = function(mode,startPosition,loop,reversed) {
if (loop == null) { loop = true; }
if (reversed == null) { reversed = false; }
	var props = new Object();
	props.mode = mode;
	props.startPosition = startPosition;
	props.labels = {};
	props.loop = loop;
	props.reversed = reversed;
	cjs.MovieClip.apply(this,[props]);

	// Слой_1
	this.shape = new cjs.Shape();
	this.shape.graphics.f("#000000").s().p("AmQOdQiVhAAAhbQAAhbCVhAQCVhBDUAAQCeAAB7AkQg0kOAAl8QAAl/A1kPQA1kPBKAAQBLAAA0EPQA1EPAAF/QAAF+g1EPQgQBTgTA6QAIAUAAAUQAABbiWBAQiVBBjSAAQjUAAiVhBg");
	this.shape.setTransform(-34,2.05);

	this.shape_1 = new cjs.Shape();
	this.shape_1.graphics.f("#000000").s().p("AhxOsQjOgViLhOQiKhOAJhYQAJhZCXgwQCYgwDOAVQCZAQB1AwQgYkMAmlyQAnl1BOkCQBPkDBIAIQBJAHAYENQAYENgmF1QgmF0hPECQgYBQgYA2QAFAUgCAUQgJBYiYAwQhpAhiDAAQg5AAg/gGg");
	this.shape_1.setTransform(-44.4011,-2.0387);

	this.shape_2 = new cjs.Shape();
	this.shape_2.graphics.f("#000000").s().p("AjLN0QjJgqiAhbQiAhbAShWQAShWCagfQCaggDJArQCUAfBuA8QAFkLBMlnQBNlrBnj2QBpj2BGAPQBHAPgFELQgEELhMFqQhNFqhoD2QggBMgdAzQADAUgEATQgSBWibAfQhBAOhKAAQhiAAhzgZg");
	this.shape_2.setTransform(-53.1725,-5.5849);

	this.shape_3 = new cjs.Shape();
	this.shape_3.graphics.f("#000000").s().p("AkwM4QjCg/h2hoQh2hpAbhTQAchUCcgPQCdgODCA/QCSAwBmBGQAhkIByldQBzlgCCjqQCCjpBEAWQBFAWghEJQghEKhzFfQhyFgiCDpQgoBIgjAwQABAUgGATQgbBTidAPQgcADgeAAQiFAAifg0g");
	this.shape_3.setTransform(-60.9984,-8.661);

	this.shape_4 = new cjs.Shape();
	this.shape_4.graphics.f("#000000").s().p("Ag7NPQifgCi9hVQi9hVhrh1Qhsh1AlhSQAkhRCfACQCfADC9BUQCOBABeBRQA+kGCYlTQCZlWCcjdQCbjcBDAdQBCAeg9EHQg9EHiZFVQiZFWicDdQgwBEgoAsQAAAUgIASQgkBQiYAAIgHAAg");
	this.shape_4.setTransform(-68.4851,-11.3967);

	this.shape_5 = new cjs.Shape();
	this.shape_5.graphics.f("#000000").s().p("AinMyQihgTi3hqQi4hphhiDQhhiCAuhPQAthPCiATQChATC4BpQCJBQBYBdQBakEC+lJQC/lMC1jQQC2jQBAAlQBBAlhaEFQhaEFi/FLQi/FLi2DRQg4BAgtApQgDAUgKASQglBAhwAAQgbAAgegEg");
	this.shape_5.setTransform(-75.805,-13.8289);

	this.timeline.addTween(cjs.Tween.get({}).to({state:[{t:this.shape}]}).to({state:[{t:this.shape_1}]},1).to({state:[{t:this.shape_2}]},1).to({state:[{t:this.shape_3}]},1).to({state:[{t:this.shape_4}]},1).to({state:[{t:this.shape_5}]},1).to({state:[{t:this.shape_4}]},1).to({state:[{t:this.shape_3}]},1).to({state:[{t:this.shape_2}]},1).to({state:[{t:this.shape_1}]},1).to({state:[{t:this.shape}]},1).wait(1));

	this._renderFirstFrame();

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-161.7,-96.9,182.7,198);


(lib.жук = function(mode,startPosition,loop,reversed) {
if (loop == null) { loop = true; }
if (reversed == null) { reversed = false; }
	var props = new Object();
	props.mode = mode;
	props.startPosition = startPosition;
	props.labels = {};
	props.loop = loop;
	props.reversed = reversed;
	cjs.MovieClip.apply(this,[props]);

	// Слой_1
	this.instance = new lib.лапа("synched",7);
	this.instance.setTransform(24.95,138.1,0.6387,0.6566,0,-90,90,5,-96.9);

	this.instance_1 = new lib.лапа("synched",4);
	this.instance_1.setTransform(39.95,40.45,0.6387,0.6566,0,-90,90,5,-96.9);

	this.instance_2 = new lib.лапа("synched",1);
	this.instance_2.setTransform(24.95,-52.85,0.6387,0.6566,0,-90,90,5,-96.9);

	this.instance_3 = new lib.лапа("synched",6);
	this.instance_3.setTransform(-41.4,138.1,0.6387,0.6566,90,0,0,5,-96.9);

	this.instance_4 = new lib.лапа("synched",3);
	this.instance_4.setTransform(-68,40.45,0.6387,0.6566,90,0,0,5,-96.9);

	this.instance_5 = new lib.лапа("synched",0);
	this.instance_5.setTransform(-46.4,-52.85,0.6387,0.6566,90,0,0,5,-96.9);

	this.instance_6 = new lib.тело("synched",0);
	this.instance_6.setTransform(-11.95,-15.15,1,1,0,0,0,0.6,-13.2);

	this.timeline.addTween(cjs.Tween.get({}).to({state:[{t:this.instance_6},{t:this.instance_5},{t:this.instance_4},{t:this.instance_3},{t:this.instance_2},{t:this.instance_1},{t:this.instance}]}).wait(10));

	this._renderFirstFrame();

}).prototype = p = new cjs.MovieClip();
p.nominalBounds = new cjs.Rectangle(-197.9,-209.9,367.8,389);


// stage content:
(lib.ZhukLife_btn_HTML5Canvas = function(mode,startPosition,loop,reversed) {
if (loop == null) { loop = true; }
if (reversed == null) { reversed = false; }
	var props = new Object();
	props.mode = mode;
	props.startPosition = startPosition;
	props.labels = {};
	props.loop = loop;
	props.reversed = reversed;
	cjs.MovieClip.apply(this,[props]);

	this.actionFrames = [0,21,23];
	this.streamSoundSymbolsList[0] = [{id:"zhukzvuk4",startFrame:0,endFrame:60,loop:1,offset:0}];
	this.streamSoundSymbolsList[21] = [{id:"sorry",startFrame:21,endFrame:60,loop:1,offset:0}];
	this.streamSoundSymbolsList[23] = [{id:"notsorry",startFrame:23,endFrame:36,loop:1,offset:0}];
	// timeline functions:
	this.frame_0 = function() {
		this.clearAllSoundStreams();
		 
		var soundInstance = playSound("zhukzvuk4",0);
		this.InsertIntoSoundStreamData(soundInstance,0,60,1);
		//остановка воспроизведения в начальном кадре
		this.stop();
		//запуск воспроизведения анимации щелчком мыши по кнопке but1
		this.start_btn.addEventListener("click",f1.bind(this));
		function f1(args){this.play();}
		//остановка воспроизведения щелчком мыши по кнопке but2 
		this.stop_btn.addEventListener("click",f2.bind(this));
		function f2(args){this.stop();}
		//переход во второй кадр анимации щелчком мыши по кнопке but3 
		this.return_btn.addEventListener("click",f3.bind(this));
		function f3(args){this.gotoAndStop(1);}
	}
	this.frame_21 = function() {
		var soundInstance = playSound("sorry",0);
		this.InsertIntoSoundStreamData(soundInstance,21,60,1);
	}
	this.frame_23 = function() {
		var soundInstance = playSound("notsorry",0);
		this.InsertIntoSoundStreamData(soundInstance,23,36,1);
	}

	// actions tween:
	this.timeline.addTween(cjs.Tween.get(this).call(this.frame_0).wait(21).call(this.frame_21).wait(2).call(this.frame_23).wait(37));

	// Кнопки
	this.return_btn = new lib._return();
	this.return_btn.name = "return_btn";
	this.return_btn.setTransform(695.8,1032.8);
	new cjs.ButtonHelper(this.return_btn, 0, 1, 1);

	this.stop_btn = new lib.stop();
	this.stop_btn.name = "stop_btn";
	this.stop_btn.setTransform(436.2,1032.8);
	new cjs.ButtonHelper(this.stop_btn, 0, 1, 1);

	this.start_btn = new lib.start();
	this.start_btn.name = "start_btn";
	this.start_btn.setTransform(173.45,1032);
	new cjs.ButtonHelper(this.start_btn, 0, 1, 1);

	this.timeline.addTween(cjs.Tween.get({}).to({state:[{t:this.start_btn},{t:this.stop_btn},{t:this.return_btn}]}).wait(60));

	// Слой_8
	this.instance = new lib.жук();
	this.instance.setTransform(36,82.4,0.8418,0.7598,134.9984,0,0,-16.1,-203);

	this.timeline.addTween(cjs.Tween.get(this.instance).to({scaleX:0.8417,scaleY:0.7596,rotation:138.7469,guide:{path:[36.1,82.5,91.5,116.7,152.3,164.4,185.2,190.1,226.9,225.4,241,237.4,300.1,288.1,393,368.1,449.1,409.5,533.9,472.2,611.7,509.1,637.1,521.1,665.5,532.1], orient:'fixed'}},21).to({scaleY:0.7597,rotation:-94.9607,guide:{path:[665.5,532.1,688.1,540.9,712.7,549.2,757.4,564.1,817.2,580,823.8,581.7,843.9,586.8], orient:'fixed'}},5).wait(1).to({regX:-14,regY:-15.2,rotation:71.6881,x:779.1,y:650.9},0).wait(1).to({scaleY:0.7596,rotation:-57.8535,x:1114.45,y:699.25},0).wait(1).to({rotation:-236.9235,x:927.55,y:564.4},0).wait(1).to({rotation:-201.8442,x:997.3,y:510.2},0).wait(1).to({rotation:-245.2885,x:906.7,y:579.4},0).wait(1).to({regX:-16.1,regY:-203.1,scaleX:0.8416,rotation:-225.0033,x:1052.15,y:642.05},0).to({regY:-203,scaleX:0.8418,scaleY:0.7598,rotation:-225.0016,x:1881.7,y:1194.4},27).wait(1));

	// Слой_3
	this.instance_1 = new lib.жук();
	this.instance_1.setTransform(1166.7,927.95,0.5294,0.6862,-14.9991,0,0,-14,-15.2);

	this.instance_2 = new lib.жук();
	this.instance_2.setTransform(465.75,758,0.235,0.2342,-150.001,0,0,-14.2,-15.2);

	this.instance_3 = new lib.жук();
	this.instance_3.setTransform(1774.35,634.6,0.3144,0.4433,14.9977,0,0,-13.8,-15);

	this.instance_4 = new lib.жук();
	this.instance_4.setTransform(1428.1,146.05,0.441,0.5732,90,0,0,-14.1,-15.2);

	this.instance_5 = new lib.жук();
	this.instance_5.setTransform(723.55,181.2,0.4998,0.4728,-14.9978,0,0,-14,-15.2);

	this.timeline.addTween(cjs.Tween.get({}).to({state:[{t:this.instance_5},{t:this.instance_4},{t:this.instance_3},{t:this.instance_2},{t:this.instance_1}]}).wait(60));

	// Слой_1
	this.instance_6 = new lib.жук();
	this.instance_6.setTransform(83.05,777.05,1,1,68.4947,0,0,-10.2,-200.5);

	this.timeline.addTween(cjs.Tween.get(this.instance_6).to({regX:0.1,regY:-194.2,rotation:75.9983,guide:{path:[83.2,777.2,163.3,723.2,258.1,677.2,410.1,603.4,619.3,541.8,734.9,507.8,831.8,487.3,945.9,463.2,1048.3,454.6,1098.7,450.4,1201,447.2,1302.5,444,1353.6,439.6,1483.1,428.6,1641.1,391,1704.4,376,1782.6,354.3,1831,340.8,1923.1,313.9], orient:'fixed'}},59).wait(1));

	this._renderFirstFrame();

}).prototype = p = new lib.AnMovieClip();
p.nominalBounds = new cjs.Rectangle(661.7,356,1291.5,893.4000000000001);
// library properties:
lib.properties = {
	id: '59AA27F817136E4AA0734754D086DF66',
	width: 1920,
	height: 1080,
	fps: 30,
	color: "#FFFFFF",
	opacity: 1.00,
	manifest: [
		{src:"sounds/notsorry.mp3?1759296071647", id:"notsorry"},
		{src:"sounds/sorry.mp3?1759296071647", id:"sorry"},
		{src:"sounds/zhukzvuk4.mp3?1759296071647", id:"zhukzvuk4"}
	],
	preloads: []
};



// bootstrap callback support:

(lib.Stage = function(canvas) {
	createjs.Stage.call(this, canvas);
}).prototype = p = new createjs.Stage();

p.setAutoPlay = function(autoPlay) {
	this.tickEnabled = autoPlay;
}
p.play = function() { this.tickEnabled = true; this.getChildAt(0).gotoAndPlay(this.getTimelinePosition()) }
p.stop = function(ms) { if(ms) this.seek(ms); this.tickEnabled = false; }
p.seek = function(ms) { this.tickEnabled = true; this.getChildAt(0).gotoAndStop(lib.properties.fps * ms / 1000); }
p.getDuration = function() { return this.getChildAt(0).totalFrames / lib.properties.fps * 1000; }

p.getTimelinePosition = function() { return this.getChildAt(0).currentFrame / lib.properties.fps * 1000; }

an.bootcompsLoaded = an.bootcompsLoaded || [];
if(!an.bootstrapListeners) {
	an.bootstrapListeners=[];
}

an.bootstrapCallback=function(fnCallback) {
	an.bootstrapListeners.push(fnCallback);
	if(an.bootcompsLoaded.length > 0) {
		for(var i=0; i<an.bootcompsLoaded.length; ++i) {
			fnCallback(an.bootcompsLoaded[i]);
		}
	}
};

an.compositions = an.compositions || {};
an.compositions['59AA27F817136E4AA0734754D086DF66'] = {
	getStage: function() { return exportRoot.stage; },
	getLibrary: function() { return lib; },
	getSpriteSheet: function() { return ss; },
	getImages: function() { return img; }
};

an.compositionLoaded = function(id) {
	an.bootcompsLoaded.push(id);
	for(var j=0; j<an.bootstrapListeners.length; j++) {
		an.bootstrapListeners[j](id);
	}
}

an.getComposition = function(id) {
	return an.compositions[id];
}


an.makeResponsive = function(isResp, respDim, isScale, scaleType, domContainers) {		
	var lastW, lastH, lastS=1;		
	window.addEventListener('resize', resizeCanvas);		
	resizeCanvas();		
	function resizeCanvas() {			
		var w = lib.properties.width, h = lib.properties.height;			
		var iw = window.innerWidth, ih=window.innerHeight;			
		var pRatio = window.devicePixelRatio || 1, xRatio=iw/w, yRatio=ih/h, sRatio=1;			
		if(isResp) {                
			if((respDim=='width'&&lastW==iw) || (respDim=='height'&&lastH==ih)) {                    
				sRatio = lastS;                
			}				
			else if(!isScale) {					
				if(iw<w || ih<h)						
					sRatio = Math.min(xRatio, yRatio);				
			}				
			else if(scaleType==1) {					
				sRatio = Math.min(xRatio, yRatio);				
			}				
			else if(scaleType==2) {					
				sRatio = Math.max(xRatio, yRatio);				
			}			
		}
		domContainers[0].width = w * pRatio * sRatio;			
		domContainers[0].height = h * pRatio * sRatio;
		domContainers.forEach(function(container) {				
			container.style.width = w * sRatio + 'px';				
			container.style.height = h * sRatio + 'px';			
		});
		stage.scaleX = pRatio*sRatio;			
		stage.scaleY = pRatio*sRatio;
		lastW = iw; lastH = ih; lastS = sRatio;            
		stage.tickOnUpdate = false;            
		stage.update();            
		stage.tickOnUpdate = true;		
	}
}
an.handleSoundStreamOnTick = function(event) {
	if(!event.paused){
		var stageChild = stage.getChildAt(0);
		if(!stageChild.paused || stageChild.ignorePause){
			stageChild.syncStreamSounds();
		}
	}
}
an.handleFilterCache = function(event) {
	if(!event.paused){
		var target = event.target;
		if(target){
			if(target.filterCacheList){
				for(var index = 0; index < target.filterCacheList.length ; index++){
					var cacheInst = target.filterCacheList[index];
					if((cacheInst.startFrame <= target.currentFrame) && (target.currentFrame <= cacheInst.endFrame)){
						cacheInst.instance.cache(cacheInst.x, cacheInst.y, cacheInst.w, cacheInst.h);
					}
				}
			}
		}
	}
}


})(createjs = createjs||{}, AdobeAn = AdobeAn||{});
var createjs, AdobeAn;