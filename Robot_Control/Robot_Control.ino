#include <Servo.h>
Servo servo01;
Servo servo02;
Servo servo03;
Servo servo04;
Servo servo05;
Servo servo06;
int servo1Pos,servo2Pos,servo3Pos,servo4Pos,servo5Pos,servo6Pos;
int servo1PPos,servo2PPos,servo3PPos,servo4PPos,servo5PPos,servo6PPos;
char a;
int d =0;
void setup() {
  Serial.begin(9600);
  servo01.attach(5);
  servo02.attach(6);
  servo03.attach(7);
  servo04.attach(8);
  servo05.attach(9);
  servo06.attach(10);
  delay(20);
  // Robot arm initial position
  servo1PPos = 56;
  servo01.write(servo1PPos);
  servo2PPos =108;
  servo02.write(servo2PPos);
  servo3PPos = 0;
  servo03.write(servo3PPos);
  servo4PPos = 90;
  servo04.write(servo4PPos);
  servo5PPos = 0;
  servo05.write(servo5PPos);
  servo6PPos = 110;
  servo06.write(servo6PPos);
  
  

}

void Set_up()
{
     for( servo4PPos; servo4PPos >= 108;servo4PPos--)
  {
    servo04.write(servo4PPos);
    delay(50);
    }
  for( servo6PPos; servo6PPos <=130 ;servo6PPos++)
  {
    servo06.write(servo6PPos);
    delay(50);
    }
         for(servo2PPos ; servo2PPos <= 108; servo2PPos++)
  {
    servo02.write(servo2PPos);
    delay(100);
    }
 
  }
void step_gap()
{
   for(servo4PPos ; servo4PPos <= 132; servo4PPos++)
  {
    servo04.write(servo4PPos);
    delay(100);
    }
  for( servo6PPos; servo6PPos >= 43;servo6PPos--)
  {
    servo06.write(servo6PPos);
    delay(100);
    }
 for(servo4PPos ; servo4PPos >= 120; servo4PPos--)
  {
    servo04.write(servo4PPos);
    delay(100);
    }
      for(servo2PPos ; servo2PPos >= 90; servo2PPos--)
  {
    servo02.write(servo2PPos);
    delay(100);
    }
    for(servo4PPos ; servo4PPos >= 115; servo4PPos--)
  {
    servo04.write(servo4PPos);
    delay(100);
    }
    for(servo1PPos ; servo1PPos>=14; servo1PPos--)
  {
    servo01.write(servo1PPos);
    delay(100);
    }
  }
void step_tron()
{
  for(servo6PPos ; servo6PPos >=1; servo6PPos--)
  {
    servo06.write(servo6PPos);
    delay(100);
    }
     for(servo1PPos ; servo1PPos <=56; servo1PPos++)
  {
    servo01.write(servo1PPos);
    delay(100);
   }
}
void step_vuong () 
 {
   for(servo6PPos ; servo6PPos <=90; servo6PPos++)
  {
    servo06.write(servo6PPos);
    delay(100);
    }
     for(servo1PPos ; servo1PPos <=56; servo1PPos++)
  {
    servo01.write(servo1PPos);
    delay(100);
  }
 }
void loop() 
{

    

  
  while(Serial.available())
  {
  a = Serial.read();  
  delay(1000);
  }  
  if(a=='2' )
  {
  step_gap();
  delay(2000);
  step_tron();
  delay(1000);
  a= "";
  }
  
   if(a=='3' )
  {
   step_gap();
  delay(2000);
  step_vuong();
  delay(1000);
  a= "";
  }
  
  Set_up();
  delay(100);
}

  
  /*servo2PPos=10;
  servo02.write(servo2PPos);
  
    for(servo1Pos =40; servo1Pos > 0; servo1Pos--)
  { 
     servo01.write(servo1Pos);
     delay(15);
  }
  delay(1500);
  }
  }*/
/*
  
  for(servo2Pos =10; servo2Pos <= 50; servo2Pos ++)
  { 
     servo02.write(servo2Pos);
     delay(10);
  }
  delay(2000);
       for(servo6Pos =0; servo6Pos <90; servo6Pos++)
  { 
     servo06.write(servo6Pos);
     delay(30);
  }
  delay(2000);
     for(servo1Pos =0; servo1Pos <40; servo1Pos++)
  { 
     servo01.write(servo1Pos);
     delay(10);
  }
  delay(1000);
      for(servo6Pos =89; servo6Pos >0; servo6Pos--)
  { 
     servo06.write(servo6Pos);
     delay(30);
  }
       for(servo2Pos=50; servo2Pos >=10; servo2Pos--)
  { 
     servo02.write(servo2Pos);
     delay(50);
  }
  
  delay(5000);*/
 
  


  
 
