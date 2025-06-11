using Microsoft.AspNetCore.Mvc;
using Api.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;


namespace Api.controller
{

    [ApiController] //diz que essa classe é um atendente da api 
    [Route("api/controller")]//o caminho que o navegador segue ate o atendente
    public class RedeController : ControllerBase // define que a rede controller extendera o controller base 
    {
        Rede rede1 = new Rede();
        [HttpPost] // Responde quando alguem envia dados
        public IActionResult PostRede([FromBodyAttribute] Rede rede)
        { // uma função do tipo  "IActionResult" que recebera os atributos do corpo e tentara colocaro em um objeto com nome "rede" da classe Rede que esta em models 
          // onde a logica é desenvolvida 


            return Ok(new
            {
                mensagem = "dados recebidos",
                dados = rede
            });
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok("API get está funcionando /n");
        }
    }
  
}