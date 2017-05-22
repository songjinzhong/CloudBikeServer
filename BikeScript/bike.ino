/*
 串口通讯简单测试
 */

// 引脚定义
const int analogInPin = A0;  // 模拟输入引脚
int potentiometerValue = 0;        // 电位器电压值
int counterValue = 0;       // counter value
int outputValuePOT = 0;        // output value
int outputValueCNT = 0;
int led = 13;               // Pin 13 has a LED
int count_wheels = 0;
unsigned long start_time;
unsigned long interrept_in_time = 0;
unsigned long current_time = 0;
unsigned long loop_time = 0;
int last_outputValuePOT=0;
float last_speed_wheels=0.0f;

float speed_wheels = 0.0f;

int pbIn = 2;          // 定义输入信号引脚
volatile int state = HIGH;      // 定义默认输入状态

void setup() {
  // 初始化串口参数
  Serial.begin(9600); 
  // initialize the digital pin as an output
  pinMode(led,OUTPUT);
  pinMode(pbIn, INPUT);
  analogWrite(11,255);
   // 监视中断输入引脚的变化
  attachInterrupt(0, stateChange, FALLING);
  start_time = millis();
}

void loop() {
  
    // 打印结果到串口监视器
    potentiometerValue = analogRead(A0);
    counterValue = analogRead(A1);
    //Serial.read();
    outputValuePOT =  map(potentiometerValue, 0, 1023, 0, 127);
    outputValueCNT =  map(counterValue, 0, 1023, 0, 127);
    
    
    if(Serial)
    {
        current_time = millis();
        if( current_time-interrept_in_time >= 1000)
       {
         speed_wheels = 0.0f;
         count_wheels = 0;
         interrept_in_time=current_time;
         Serial.print("0");
         Serial.print("b");
       }
       if(abs(last_speed_wheels-speed_wheels)>5){
          last_speed_wheels=speed_wheels;
          //last_outputValuePOT=outputValuePOT;
          //Serial.print("33");
          //Serial.print("a");
          Serial.print(speed_wheels);
          Serial.print("b");
       }
        
    }
    // 取保能稳定读取下一次数值
    delay(100);  
}

void stateChange()
{
  interrept_in_time = millis();
  unsigned long now = millis();
  loop_time = now - start_time;
  speed_wheels = 10000/(float)loop_time;
  start_time = millis();
}
