# 🚀 .NET 6 Logging with ELK (Elasticsearch, Logstash, Kibana)

This project demonstrates **.NET 6 logging** using **Serilog** and the **ELK stack**.

## 📌 Features
- ✅ Structured logging with **Serilog**
- ✅ Log storage in **Elasticsearch**
- ✅ Log processing with **Logstash**
- ✅ Log visualization using **Kibana**
- ✅ Docker-based setup for easy deployment

---

## 📂 Project Structure
```
MySolution/
│── MySolution.sln         # Visual Studio Solution File
│
└─── ELKLoggingDemo/
    │── ELKLoggingDemo.csproj  # .NET project file
    │── Program.cs             # Main .NET app
    │── appsettings.json       # Config file (Optional)
    │── Dockerfile             # (Optional) Containerization setup
    │
    ├───Properties/
    │   └── launchSettings.json  # Debugging settings
    │
    ├───bin/                     # Build files (Auto-generated)
    ├───obj/                     # Build artifacts (Auto-generated)
    │
    └───LoggingSetup/            # ELK Setup
        │── docker-compose.yml   # Starts Elasticsearch, Logstash, Kibana
        │── logstash.conf        # Logstash pipeline configuration
        │── filebeat.yml         # (Optional) Sends logs from files
```

---

## 🔧 **Setup Instructions**
### **1️⃣ Install Required Software**
- **[Docker](https://www.docker.com/)**
- **[Visual Studio](https://visualstudio.microsoft.com/)**
- **[Postman](https://www.postman.com/)** (Optional)

---

### **2️⃣ Start ELK Stack**
Run the following command in the **`LoggingSetup/`** folder:
```sh
docker-compose up -d
```
✅ This starts:
- **Elasticsearch** → `http://localhost:9200`
- **Logstash** → Port `5044`
- **Kibana** → `http://localhost:5601`

---

### **3️⃣ Run .NET 6 App**
1. Open **Visual Studio**
2. Press **`F5`** to start the API
3. Send test requests:
```sh
curl http://localhost:5000/weatherforecast
curl http://localhost:5000/error
```
✅ Logs will be sent to **Elasticsearch**.

---

### **4️⃣ View Logs in Kibana**
1. Open **Kibana** → [http://localhost:5601](http://localhost:5601)
2. Click on **Discover**
3. Create an **Index Pattern** → `dotnet-logs-*`
4. Search logs by **timestamp**, **error level**, or **message**

---

## 🔍 **Checking Logs Manually**
### **Check If Elasticsearch is Running**
```sh
curl http://localhost:9200
```
✅ If Elasticsearch is running, you will see server details.

### **List Available Log Indexes**
```sh
curl http://localhost:9200/_cat/indices?v
```
✅ Expected output:
```
health status index                uuid                   pri rep docs.count
yellow open   dotnet-logs-2025.01.12 W8GyXkH-5f-6MvV       1   1    100
```

### **Search All Logs**
```sh
curl http://localhost:9200/dotnet-logs-*/_search?pretty=true
```
✅ Shows **all logs** stored in Elasticsearch.

### **Search Only `Error` Logs**
```json
GET dotnet-logs-*/_search
{
  "query": {
    "match": {
      "level": "Error"
    }
  }
}
```
✅ Retrieves **only error logs**.

### **Search Logs from the Last 1 Hour**
```json
GET dotnet-logs-*/_search
{
  "query": {
    "range": {
      "@timestamp": {
        "gte": "now-1h",
        "lt": "now"
      }
    }
  }
}
```
✅ Retrieves logs **from the last 1 hour**.

### **Search Logs from the Last 2 Days**
```json
GET dotnet-logs-*/_search
{
  "query": {
    "range": {
      "@timestamp": {
        "gte": "now-2d",
        "lt": "now"
      }
    }
  }
}
```
✅ Retrieves logs **from the last 2 days**.

---

## 🛠 **Troubleshooting**
| Issue | Possible Fix |
|--------|------------|
| **No logs in Kibana** | Check if Elasticsearch is running: `curl http://localhost:9200` |
| **Logstash not processing logs** | Run `docker logs logstash` |
| **.NET app not sending logs** | Ensure Serilog is set up in `Program.cs` |
| **Kibana doesn’t show logs** | Create an **Index Pattern** for `dotnet-logs-*` |



