import { Component, OnInit } from '@angular/core';
import { PositionService } from './position.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  title = 'app';

 
  constructor(private positionService: PositionService) {
  }

  x: number;
  y:number;
  z:number;

  y0: number = 0;
  rotate0: number = 0;

  eventstring: string = "not set";

  _this: AppComponent = this;



  ngOnInit(): void{
    window.addEventListener("devicemotion",(event) =>  this.handleMotionEvent (this, event), true);


    window.addEventListener("deviceorientation",(event) =>  this.handleOrientationEvent (this, event), true);
  }

  handleMotionEvent(_this, event){
    _this.eventstring = JSON.stringify(event);
    console.log(this.eventstring);
    _this.x = event.accelerationIncludingGravity.x;
    _this.y = event.accelerationIncludingGravity.y;
    _this.z = event.accelerationIncludingGravity.z;

    _this.positionService.sendY(event.accelerationIncludingGravity.y - _this.y0);

    
  }

  rotateDegrees: number;
  leftToRight: number;
  frontToBack: number;

  lastSent = Date.now();

  handleOrientationEvent (_this: AppComponent, event){
    // alpha: rotation around z-axis
    _this.rotateDegrees = event.alpha;
    if (_this.rotateDegrees <0){
      _this.rotateDegrees +=360;
    }
    // gamma: left to right
    _this.leftToRight = event.gamma;
    // beta: front back motion
    _this.frontToBack = event.beta;

    if (_this.isOn || (Date.now() - this.lastSent) > 1000){
      _this.positionService.sendRotate(event.alpha - _this.rotate0);
      this.lastSent = Date.now();
    }
  }

  isOn = true;

  turnOn(){
    this.isOn = true;
    this.positionService.sendDimm(true);
    this.center();
  }

  turnOff(){
    this.positionService.sendDimm(false);
    this.isOn = false;
  }

  center(){
    this.y0 = this.y;
    this.rotate0 = this.rotateDegrees;
  }
}
